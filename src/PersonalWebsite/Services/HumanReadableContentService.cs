﻿using PersonalWebsite.Lib;
using PersonalWebsite.Repositories;
using PersonalWebsite.ViewModels.Content;
using System;

namespace PersonalWebsite.Services
{
    public class HumanReadableContentService : IHumanReadableContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly IPageConfiguration _pageConfiguration;

        private bool _isDisposed = false;

        public HumanReadableContentService(
            IPageConfiguration pageConfiguration,
            IContentRepository contentRespository)
        {
            if(pageConfiguration == null)
            {
                throw new ArgumentNullException(nameof(pageConfiguration));
            }
            
            if(contentRespository == null)
            {
                throw new ArgumentNullException(nameof(contentRespository));
            }
            _contentRepository = contentRespository;
            _pageConfiguration = pageConfiguration;
        }

        /// <summary>
        /// Get translated page by its internal caption.
        /// </summary>
        /// <param name="languageDefinition">Required language definition.</param>
        /// <param name="internalCaption">Internal caption of a content.</param>
        /// <returns><typeparamref name="PageViewModel"/> instance</returns>
        public PageViewModel GetPageByInternalCaption(LanguageDefinition languageDefinition, string internalCaption)
        {
            return GetPageViewModelBy(languageDefinition, () => _contentRepository.FindTranslatedContentByInternalCaption(languageDefinition, internalCaption));
        }

        /// <summary>
        /// Get translated page by its translated human-readable name.
        /// </summary>
        /// <param name="languageDefinition">Required language definition.</param>
        /// <param name="urlName">Human-readable name of a content.</param>
        /// <returns><typeparamref name="PageViewModel"/> instance</returns>
        public PageViewModel GetPageByHumanReadableName(LanguageDefinition languageDefinition, string urlName)
        {
            return GetPageViewModelBy(languageDefinition, () => _contentRepository.FindTranslatedContentByUrlName(languageDefinition, urlName));
        }

        ~HumanReadableContentService() {
            Dispose();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _contentRepository.Dispose();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private void GuardNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ContentRepository));
            }
        }

        private PageViewModel GetPageViewModelBy(LanguageDefinition languageDefinition, Func<ContentViewModel> contentViewModelMethod)
        {
            GuardNotDisposed();

            ContentViewModel contentVM;
            ContentLinksViewModel linksVM;
            using (_contentRepository)
            {
                contentVM = contentViewModelMethod();

                if (contentVM == null)
                {
                    return null; // No content available.
                }

                linksVM = _contentRepository.GetContentLinksPresentationData(languageDefinition,
                    Enum.GetNames(typeof(PredefinedPages))
                );
            }

            return new PageViewModel(_pageConfiguration, languageDefinition, contentVM, linksVM);
        }
    }
}
