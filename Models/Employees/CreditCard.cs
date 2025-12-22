using Seido.Utilities.SeedGenerator;

namespace Models.Employees;

public enum CardIssuer {AmericanExpress, Visa, MasterCard, DinersClub}
public record CreditCard (
    Guid CreditCardId,
    CardIssuer Issuer,
    string Number,
    string ExpirationYear,
    string ExpirationMonth,
    bool Seeded = false
) : ISeed<CreditCard>
{
    public CreditCard() : this(default, default, default, default, default) {}
 
    #region randomly seed this instance
    public virtual CreditCard Seed (SeedGenerator seeder)
    {
        var ret = new CreditCard(
            Guid.NewGuid(),
            seeder.FromEnum<CardIssuer>(),
            $"{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}",
            $"{seeder.Next(25, 32)}",
            $"{seeder.Next(01, 13):D2}",
            true
        );
        return ret;
    }
    #endregion
}