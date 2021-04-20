namespace ScorpPlus.Dtos
{
    public class IndividualInfoRequestResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
    }
    
    public class IndividualInfoResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IndividualInfoData Data { get; set; }
    }
    
    public class IndividualInfoData
    {
        public IndividualInfoBasicFl BasicFl { get; set; }
        public IndividualInfoReliabilityFl ReliabilityFl { get; set; }
    }

    public class IndividualInfoBasicFl
    {
        public string Iin { get; set; }
        public long Rnn { get; set; }
        public string Name { get; set; }
        public string SourceLink { get; set; }
    }

    public class IndividualInfoReliabilityFl
    {
        public bool Terrorist { get; set; }
        public bool Pedophile { get; set; }
        public bool AlimonyPayer { get; set; }
    }
}