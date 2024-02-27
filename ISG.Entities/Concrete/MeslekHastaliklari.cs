using ISG.Entities.Abstract;


namespace ISG.Entities.Concrete
{
    public class MeslekHastaliklari : IEntity
    {
        public int MeslekHastaliklari_Id { get; set; }
        public string meslekHastalik { get; set; }
        public string grubu { get; set; }
        public string sure { get; set; }
    }
}
