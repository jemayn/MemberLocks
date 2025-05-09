namespace MemberLoginTest.SeedMembers;

public class MemberSeedData
{
    public List<MemberData> SeedMembers()
    {
        var members = new List<MemberData>();

        var random = new Random();
        var memberGroups = new[] { "User", "Super User", "Translator" };

        for (var i = 1; i <= 1000; i++)
        {
            var member = new MemberData
            {
                Username = $"user{i}",
                Email = $"user{i}@example.com",
                MemberGroup = [memberGroups[random.Next(memberGroups.Length)]]
            };

            members.Add(member);
        }

        return members;
    }
}
