namespace PythogorasSquare.Web.DomainModel
{
    public class QualityDetailedInfo
    {
        public int QualityPower { get; set; }

        public int QualityId { get; set; }

        public virtual Quality Quality { get; set; }

        public int QualityDescriptionId { get; set; }

        public virtual QualityDescription QualityDescription { get; set; }
    }
}