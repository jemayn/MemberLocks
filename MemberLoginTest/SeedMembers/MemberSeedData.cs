namespace MemberLoginTest.SeedMembers;

public class MemberSeedData
{
    public List<MemberData> SeedMembers()
    {
        var members = new List<MemberData>();
        
        for (var i = 1; i <= 1000; i++)
        {
            var member = new MemberData
            {
                Username = $"user{i}",
                Email = $"user{i}@example.com",
            };

            members.Add(member);
        }

        return members;
    }
}
