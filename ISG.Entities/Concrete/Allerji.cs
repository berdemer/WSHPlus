using ISG.Entities.Abstract;

namespace ISG.Entities.Concrete
{
    public partial class Allerji : IEntity
    {
        public int Allerji_Id { get; set; }
        public string AllerjiOykusu { get; set; }
        public string AllerjiCesiti { get; set;}//deri rinit astým anaflaksi
        public string AllerjiEtkeni { get; set; }//süt penisilin güneþ
        public int AllerjiSuresi { get; set; }//yýl süresi
        public int Personel_Id { get; set; }
        public string UserId { get; set; }
        public virtual Personel Personel { get; set; }
    }
}
