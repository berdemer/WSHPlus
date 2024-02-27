using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public partial class Allerji : IEntity
    {
        public int Allerji_Id { get; set; }
        public string AllerjiOykusu { get; set; }
        public string AllerjiCesiti { get; set;}//deri rinit ast�m anaflaksi
        public string AllerjiEtkeni { get; set; }//s�t penisilin g�ne�
        public int AllerjiSuresi { get; set; }//y�l s�resi
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
