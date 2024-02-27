using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public class KtubKt : IEntity
    {
        public int KtubKt_Id { get; set; }
        public string Name { get; set; }
        public string Element { get; set; }
        public string FirmName { get; set; }
        public string ConfirmationDateKub { get; set; }
        public string ConfirmationDateKt { get; set; }
        public string DocumentPathKub { get; set; }
        public string DocumentPathKt { get; set; }
    }
}
