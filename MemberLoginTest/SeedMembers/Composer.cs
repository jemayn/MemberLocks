using Umbraco.Cms.Core.Composing;

namespace MemberLoginTest.SeedMembers;

public class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<MemberSeedData>();
    }
}