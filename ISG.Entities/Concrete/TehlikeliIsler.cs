using ISG.Entities.Abstract;


namespace ISG.Entities.Concrete
{
    public class TehlikeliIsler : IEntity
    {
        public int TehlikeliIsler_Id { get; set; }
        public string meslek { get; set; }
        public string grubu { get; set; }
        public string sure { get; set; }
    }
}
