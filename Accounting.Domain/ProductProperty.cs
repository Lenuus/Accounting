namespace Accounting.Domain
{
    public class ProductProperty: IBaseEntity, ISoftDeletable, ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedDate { get ; set ; }
        public Guid? DeletedById { get ; set ; }
    }
}