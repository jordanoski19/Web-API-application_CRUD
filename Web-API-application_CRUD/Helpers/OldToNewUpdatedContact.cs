namespace API.Helpers
{
    public class OldToNewUpdatedContact
    {
        public string OldName { get; set; }
        public int OldCompanyId { get; set; }
        public int OldCountryId { get; set; }

        public string NewName { get; set; }
        public int NewCompanyId { get; set; }
        public int NewCountryId { get; set; }
    }
}
