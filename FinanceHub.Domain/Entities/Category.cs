using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class Category : BaseEntity
{
    public Category(string name, ECategoryType categoryType, int userId)
    {
        Name = name;
        CategoryType = categoryType;
        UserId = userId;
    }

    public string Name { get; private set; }
    public ECategoryType CategoryType { get; private set; }
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;
    public virtual ICollection<Bill> Bills { get; private set; } = new List<Bill>();

    public void Update(string name, ECategoryType? categoryType)
    {
        Name = name;
        if (categoryType.HasValue) CategoryType = categoryType.Value;
    }
}