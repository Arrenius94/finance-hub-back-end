using FinanceHub.Domain.Enums;

namespace FinanceHub.Domain.Entities;

public class Category : BaseEntity
{
    public Category(string name, ECategoryType categoryType, int userId)
    {
        Name = name;
        CategoryType = categoryType;
        UserId = userId;

        Bills = new List<Bill>();
    }

    public string Name { get; private set; }
    public ECategoryType CategoryType { get; private set; }
    public int UserId { get; private set; }
    public virtual User User { get; private set; }
    public virtual ICollection<Bill> Bills { get; private set; }
}