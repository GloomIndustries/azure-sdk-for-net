﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using NUnit.Framework;

namespace Azure.AI.FormRecognizer.DocumentAnalysis.Tests
{
    /// <summary>
    /// The suite of tests for the document model methods in the <see cref="DocumentModelAdministrationClient"/> class.
    /// </summary>
    /// <remarks>
    /// These tests have a dependency on live Azure services and may incur costs for the associated
    /// Azure subscription.
    /// </remarks>
    [IgnoreServiceError(400, "InvalidRequest", Message = "Content is not accessible: Invalid data URL", Reason = "https://github.com/Azure/azure-sdk-for-net/issues/28923")]
    public class DocumentModelAdministrationLiveTests : DocumentAnalysisLiveTestBase
    {
        private readonly IReadOnlyDictionary<string, string> _testingTags = new Dictionary<string, string>()
        {
            { "ordinary tag", "an ordinary tag" },
            { "crazy tag", "a CRAZY tag 123!@#$%^&*()[]{}\\/?.,<>" }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentModelAdministrationLiveTests"/> class.
        /// </summary>
        /// <param name="isAsync">A flag used by the Azure Core Test Framework to differentiate between tests for asynchronous and synchronous methods.</param>
        public DocumentModelAdministrationLiveTests(bool isAsync, DocumentAnalysisClientOptions.ServiceVersion serviceVersion)
            : base(isAsync, serviceVersion)
        {
        }

        #region Build

        [RecordedTest]
        public async Task BuildModelCanAuthenticateWithTokenCredential()
        {
            var client = CreateDocumentModelAdministrationClient(useTokenCredential: true);
            var trainingFilesUri = new Uri(TestEnvironment.BlobContainerSasUrl);

            var modelId = Recording.GenerateId();
            BuildDocumentModelOperation operation = await client.BuildDocumentModelAsync(WaitUntil.Completed, trainingFilesUri, DocumentBuildMode.Template, modelId);

            // Sanity check to make sure we got an actual response back from the service.
            Assert.IsNotNull(operation.Value.ModelId);

            await client.DeleteDocumentModelAsync(modelId);
        }

        [RecordedTest]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BuildModel(bool singlePage)
        {
            var client = CreateDocumentModelAdministrationClient();
            var trainingFilesUri = new Uri(singlePage ? TestEnvironment.BlobContainerSasUrl : TestEnvironment.MultipageBlobContainerSasUrl);
            var modelId = Recording.GenerateId();

            BuildDocumentModelOperation operation = await client.BuildDocumentModelAsync(WaitUntil.Completed, trainingFilesUri, DocumentBuildMode.Template, modelId);

            Assert.IsTrue(operation.HasValue);

            DocumentModelDetails model = operation.Value;

            ValidateDocumentModelDetails(model);

            Assert.AreEqual(1, model.DocumentTypes.Count);
            Assert.IsTrue(model.DocumentTypes.ContainsKey(modelId));

            DocumentTypeDetails documentType = model.DocumentTypes[modelId];

            Assert.AreEqual(DocumentBuildMode.Template, documentType.BuildMode);
        }

        [RecordedTest]
        public async Task BuildModelWithNeuralBuildMode()
        {
            // Test takes too long to finish running, and seems to cause multiple failures in our
            // live test pipeline. Until we find a way to run it without flakiness, this test will
            // be ignored when running in Live mode.

            if (Recording.Mode == RecordedTestMode.Live)
            {
                Assert.Ignore("https://github.com/Azure/azure-sdk-for-net/issues/27042");
            }

            var client = CreateDocumentModelAdministrationClient();
            var trainingFilesUri = new Uri(TestEnvironment.BlobContainerSasUrl);
            var modelId = Recording.GenerateId();

            BuildDocumentModelOperation operation = await client.BuildDocumentModelAsync(WaitUntil.Started, trainingFilesUri, DocumentBuildMode.Neural, modelId);

            await operation.WaitForCompletionAsync(TimeSpan.FromMinutes(1));

            Assert.IsTrue(operation.HasValue);

            DocumentModelDetails model = operation.Value;

            ValidateDocumentModelDetails(model);

            Assert.AreEqual(1, model.DocumentTypes.Count);
            Assert.IsTrue(model.DocumentTypes.ContainsKey(modelId));

            DocumentTypeDetails documentType = model.DocumentTypes[modelId];

            Assert.AreEqual(DocumentBuildMode.Neural, documentType.BuildMode);
        }

        [RecordedTest]
        public async Task BuildModelSucceedsWithValidPrefix()
        {
            var client = CreateDocumentModelAdministrationClient();
            var trainingFilesUri = new Uri(TestEnvironment.BlobContainerSasUrl);
            var modelId = Recording.GenerateId();

            BuildDocumentModelOperation operation = await client.BuildDocumentModelAsync(WaitUntil.Completed, trainingFilesUri, DocumentBuildMode.Template, modelId, "subfolder/");

            Assert.IsTrue(operation.HasValue);
            Assert.IsNotNull(operation.Value.ModelId);
        }

        [RecordedTest]
        [Ignore("https://github.com/azure/azure-sdk-for-net/issues/28272")]
        public void BuildModelFailsWithInvalidPrefix()
        {
            var client = CreateDocumentModelAdministrationClient();
            var trainingFilesUri = new Uri(TestEnvironment.BlobContainerSasUrl);
            var modelId = Recording.GenerateId();

            RequestFailedException ex = Assert.ThrowsAsync<RequestFailedException>(async () => await client.BuildDocumentModelAsync(WaitUntil.Started, trainingFilesUri, DocumentBuildMode.Template, modelId, "subfolder"));
            Assert.AreEqual("InvalidRequest", ex.ErrorCode);
        }

        [RecordedTest]
        public async Task BuildModelWithTags()
        {
            var client = CreateDocumentModelAdministrationClient();
            var trainingFilesUri = new Uri(TestEnvironment.BlobContainerSasUrl);
            var modelId = Recording.GenerateId();

            var options = new BuildDocumentModelOptions();

            foreach (var tag in _testingTags)
            {
                options.Tags.Add(tag);
            }

            BuildDocumentModelOperation operation = await client.BuildDocumentModelAsync(WaitUntil.Completed, trainingFilesUri, DocumentBuildMode.Template, modelId, options: options);

            DocumentModelDetails model = operation.Value;

            CollectionAssert.AreEquivalent(_testingTags, model.Tags);
        }

        [RecordedTest]
        public void BuildModelError()
        {
            var client = CreateDocumentModelAdministrationClient();
            var modelId = Recording.GenerateId();

            var containerUrl = new Uri("https://someUrl");

            RequestFailedException ex = Assert.ThrowsAsync<RequestFailedException>(async () => await client.BuildDocumentModelAsync(WaitUntil.Started, containerUrl, DocumentBuildMode.Template, modelId));
            Assert.AreEqual("InvalidArgument", ex.ErrorCode);
        }

        #endregion

        #region Copy

        [RecordedTest]
        [TestCase(true)]
        [TestCase(false)]
        public async Task CopyModelTo(bool useTokenCredential)
        {
            var sourceClient = CreateDocumentModelAdministrationClient(useTokenCredential);
            var targetClient = CreateDocumentModelAdministrationClient(useTokenCredential);
            var modelId = Recording.GenerateId();

            await using var trainedModel = await BuildDisposableDocumentModelAsync(modelId);

            var targetModelId = Recording.GenerateId();
            DocumentModelCopyAuthorization targetAuth = await targetClient.GetCopyAuthorizationAsync(targetModelId);

            CopyDocumentModelToOperation operation = await sourceClient.CopyDocumentModelToAsync(WaitUntil.Completed, trainedModel.ModelId, targetAuth);

            Assert.IsTrue(operation.HasValue);

            DocumentModelDetails copiedModel = operation.Value;

            ValidateDocumentModelDetails(copiedModel);
            Assert.AreEqual(targetAuth.TargetModelId, copiedModel.ModelId);
            Assert.AreNotEqual(trainedModel.ModelId, copiedModel.ModelId);

            Assert.AreEqual(1, copiedModel.DocumentTypes.Count);
            Assert.IsTrue(copiedModel.DocumentTypes.ContainsKey(modelId));

            DocumentTypeDetails documentType = copiedModel.DocumentTypes[modelId];

            Assert.AreEqual(DocumentBuildMode.Template, documentType.BuildMode);
        }

        [RecordedTest]
        public async Task CopyModelToWithTags()
        {
            var sourceClient = CreateDocumentModelAdministrationClient();
            var targetClient = CreateDocumentModelAdministrationClient();
            var modelId = Recording.GenerateId();

            await using var trainedModel = await BuildDisposableDocumentModelAsync(modelId);

            var tags = _testingTags.ToDictionary(t => t.Key, t => t.Value);

            var targetModelId = Recording.GenerateId();
            DocumentModelCopyAuthorization targetAuth = await targetClient.GetCopyAuthorizationAsync(targetModelId, tags: tags);
            CopyDocumentModelToOperation operation = await sourceClient.CopyDocumentModelToAsync(WaitUntil.Completed, trainedModel.ModelId, targetAuth);

            DocumentModelDetails copiedModel = operation.Value;

            CollectionAssert.AreEquivalent(_testingTags, copiedModel.Tags);

            await sourceClient.DeleteDocumentModelAsync(targetModelId);
        }

        [RecordedTest]
        public async Task CopyModelToErrorAsync()
        {
            var sourceClient = CreateDocumentModelAdministrationClient();
            var targetClient = CreateDocumentModelAdministrationClient();

            var modelId = Recording.GenerateId();
            DocumentModelCopyAuthorization targetAuth = await targetClient.GetCopyAuthorizationAsync(modelId);

            RequestFailedException ex = Assert.ThrowsAsync<RequestFailedException>(async () => await sourceClient.CopyDocumentModelToAsync(WaitUntil.Started, modelId, targetAuth));
            Assert.AreEqual("InvalidRequest", ex.ErrorCode);
        }

        #endregion Copy

        #region Compose

        [RecordedTest]
        [TestCase(false)]
        [TestCase(true)]
        public async Task ComposeModel(bool useTokenCredential)
        {
            var client = CreateDocumentModelAdministrationClient(useTokenCredential);

            var modelAId = Recording.GenerateId();
            var modelBId = Recording.GenerateId();

            await using var trainedModelA = await BuildDisposableDocumentModelAsync(modelAId);
            await using var trainedModelB = await BuildDisposableDocumentModelAsync(modelBId);

            var modelIds = new List<string> { trainedModelA.ModelId, trainedModelB.ModelId };

            var composedModelId = Recording.GenerateId();
            ComposeDocumentModelOperation operation = await client.ComposeDocumentModelAsync(WaitUntil.Completed, modelIds, composedModelId);

            Assert.IsTrue(operation.HasValue);

            DocumentModelDetails composedModel = operation.Value;

            ValidateDocumentModelDetails(composedModel);

            Assert.AreEqual(2, composedModel.DocumentTypes.Count);
            Assert.IsTrue(composedModel.DocumentTypes.ContainsKey(modelAId));
            Assert.IsTrue(composedModel.DocumentTypes.ContainsKey(modelBId));

            DocumentTypeDetails documentTypeA = composedModel.DocumentTypes[modelAId];
            DocumentTypeDetails documentTypeB = composedModel.DocumentTypes[modelBId];

            Assert.AreEqual(DocumentBuildMode.Template, documentTypeA.BuildMode);
            Assert.AreEqual(DocumentBuildMode.Template, documentTypeB.BuildMode);
        }

        [RecordedTest]
        public async Task ComposeModelWithTags()
        {
            var client = CreateDocumentModelAdministrationClient();

            var modelAId = Recording.GenerateId();
            var modelBId = Recording.GenerateId();

            await using var trainedModelA = await BuildDisposableDocumentModelAsync(modelAId);
            await using var trainedModelB = await BuildDisposableDocumentModelAsync(modelBId);

            var modelIds = new List<string> { trainedModelA.ModelId, trainedModelB.ModelId };
            var tags = _testingTags.ToDictionary(t => t.Key, t => t.Value);

            var composedModelId = Recording.GenerateId();
            ComposeDocumentModelOperation operation = await client.ComposeDocumentModelAsync(WaitUntil.Completed, modelIds, composedModelId, tags: tags);

            DocumentModelDetails composedModel = operation.Value;

            CollectionAssert.AreEquivalent(_testingTags, composedModel.Tags);

            await client.DeleteDocumentModelAsync(composedModelId);
        }

        [RecordedTest]
        public async Task ComposeModelFailsWithInvalidId()
        {
            var client = CreateDocumentModelAdministrationClient();

            var modelId = Recording.GenerateId();

            await using var trainedModel = await BuildDisposableDocumentModelAsync(modelId);

            var modelIds = new List<string> { trainedModel.ModelId, "00000000-0000-0000-0000-000000000000" };

            var composedModelId = Recording.GenerateId();
            RequestFailedException ex = Assert.ThrowsAsync<RequestFailedException>(async () => await client.ComposeDocumentModelAsync(WaitUntil.Started, modelIds, composedModelId));
            Assert.AreEqual("InvalidRequest", ex.ErrorCode);
        }

        #endregion

        #region Get

        [RecordedTest]
        public async Task GetPrebuiltModel()
        {
            var client = CreateDocumentModelAdministrationClient();

            DocumentModelDetails model = await client.GetDocumentModelAsync("prebuilt-businessCard");

            ValidateDocumentModelDetails(model);
            Assert.NotNull(model.Description);
        }

        #endregion

        #region Delete

        [RecordedTest]
        public void DeleteModelFailsWhenModelDoesNotExist()
        {
            var client = CreateDocumentModelAdministrationClient();
            var fakeModelId = "00000000-0000-0000-0000-000000000000";

            RequestFailedException ex = Assert.ThrowsAsync<RequestFailedException>(async () => await client.DeleteDocumentModelAsync(fakeModelId));
            Assert.AreEqual("NotFound", ex.ErrorCode);
        }

        #endregion

        [RecordedTest]
        public async Task GetCopyAuthorization()
        {
            var targetClient = CreateDocumentModelAdministrationClient();
            var modelId = Recording.GenerateId();

            DocumentModelCopyAuthorization targetAuth = await targetClient.GetCopyAuthorizationAsync(modelId);

            Assert.IsNotNull(targetAuth.TargetModelId);
            Assert.AreEqual(modelId, targetAuth.TargetModelId);
            Assert.IsNotNull(targetAuth.AccessToken);
            Assert.IsNotNull(targetAuth.TargetResourceId);
            Assert.IsNotNull(targetAuth.TargetResourceRegion);
        }

        private void ValidateDocumentModelDetails(DocumentModelDetails model, string description = null, IReadOnlyDictionary<string, string> tags = null)
        {
            if (description != null)
            {
                Assert.AreEqual(description, model.Description);
            }

            if (tags != null)
            {
                CollectionAssert.AreEquivalent(tags, model.Tags);
            }

            Assert.IsNotNull(model.ModelId);
            Assert.AreNotEqual(default(DateTimeOffset), model.CreatedOn);

            if (_serviceVersion >= DocumentAnalysisClientOptions.ServiceVersion.V2023_02_28_Preview)
            {
                if (model.ExpiresOn.HasValue)
                {
                    Assert.Greater(model.ExpiresOn, model.CreatedOn);
                }
            }
            else
            {
                Assert.Null(model.ExpiresOn);
            }
        }
    }
}
