using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common.Controllers;

namespace MemberLoginTest.SeedMembers;

public class SeedMembersController : UmbracoApiController
{
    private readonly MemberSeedData _memberSeedData;
    private readonly IMemberService _memberService;
    private readonly IScopeProvider _scopeProvider;

    public SeedMembersController(
        MemberSeedData memberSeedData,
        IMemberService memberService,
        IScopeProvider scopeProvider
    )
    {
        _memberSeedData = memberSeedData;
        _memberService = memberService;
        _scopeProvider = scopeProvider;
    }

    public List<MemberData> GetMemberData()
    {
        var members = _memberSeedData.SeedMembers();

        foreach (var member in members)
        {
            if (_memberService.Exists(member.Email))
            {
                using var updateScope = _scopeProvider.CreateScope();
                UpdateMember(member);
                updateScope.Complete();
            }

            using var createScope = _scopeProvider.CreateScope();
            CreateMember(member);
            createScope.Complete();
        }

        return members;
    }

    private IMember? CreateMember(MemberData member)
    {
        var memberWithIdentity = _memberService.CreateMemberWithIdentity(
            member.Email,
            member.Email,
            member.Username,
            Umbraco.Cms.Core.Constants.Security.DefaultMemberTypeAlias,
            false
        );

        _memberService.Save(memberWithIdentity);

        _memberService.ReplaceRoles([memberWithIdentity.Username], member.MemberGroup.ToArray());

        return memberWithIdentity;
    }

    private IMember? UpdateMember(MemberData member)
    {
        var existingMember = _memberService.GetByEmail(member.Email);

        if (existingMember is null)
        {
            return null;
        }

        existingMember.Username = member.Email;
        existingMember.Name = member.Username;
        existingMember.Email = member.Email;
        _memberService.Save(existingMember);
        _memberService.ReplaceRoles([existingMember.Username], member.MemberGroup.ToArray());

        return existingMember;
    }
}

public class MemberData
{
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> MemberGroup { get; set; }
}
