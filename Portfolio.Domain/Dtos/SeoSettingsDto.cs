namespace Portfolio.Domain.Dtos
{
    public class SeoSettingsDto
    {
        public string Title { get; set; }

        public string DefaultMetaKeywords { get; set; }

        public string DefaultMetaDescription { get; set; }

        public bool UseTwitterMetaTags { get; set; }

        public bool UseOpenGraphMetaTags { get; set; }
    }
}
