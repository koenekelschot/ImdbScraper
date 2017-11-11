using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImdbScraper.Test
{
    [TestClass]
    public class ImdbHelperTest
    {
        [TestMethod]
        public void Imdb_Helper_IsValidTitleId()
        {
            ImdbHelper.IsValidTitleId("invalid").Should().BeFalse();
            ImdbHelper.IsValidTitleId(string.Empty).Should().BeFalse();
            ImdbHelper.IsValidTitleId(null).Should().BeFalse();
            ImdbHelper.IsValidTitleId("tt1211837").Should().BeTrue("the id of 'Doctor Strange'");
            ImdbHelper.IsValidTitleId("t1211837").Should().BeFalse();
            ImdbHelper.IsValidTitleId("1211837").Should().BeFalse();
            ImdbHelper.IsValidTitleId("tt121183").Should().BeFalse();
            ImdbHelper.IsValidTitleId("tt 1211837").Should().BeFalse();
            ImdbHelper.IsValidTitleId("tt-1211837").Should().BeFalse();
            ImdbHelper.IsValidTitleId("tt 121183").Should().BeFalse();
            ImdbHelper.IsValidTitleId("tt-121183").Should().BeFalse();
            ImdbHelper.IsValidTitleId("nm1212722").Should().BeFalse("id of a person");
            ImdbHelper.IsValidTitleId("ch0387980").Should().BeFalse("id of a character");
        }

        [TestMethod]
        public void Imdb_Helper_IsValidPersonId()
        {
            ImdbHelper.IsValidPersonId("invalid").Should().BeFalse();
            ImdbHelper.IsValidPersonId(string.Empty).Should().BeFalse();
            ImdbHelper.IsValidPersonId(null).Should().BeFalse();
            ImdbHelper.IsValidPersonId("nm1212722").Should().BeTrue("the id of 'Benedict Cumberbatch'");
            ImdbHelper.IsValidPersonId("mn1212722").Should().BeFalse();
            ImdbHelper.IsValidPersonId("m1212722").Should().BeFalse();
            ImdbHelper.IsValidPersonId("1212722").Should().BeFalse();
            ImdbHelper.IsValidPersonId("nm121272").Should().BeFalse();
            ImdbHelper.IsValidPersonId("nm 1212722").Should().BeFalse();
            ImdbHelper.IsValidPersonId("nm-1212722").Should().BeFalse();
            ImdbHelper.IsValidPersonId("nm 121272").Should().BeFalse();
            ImdbHelper.IsValidPersonId("nm-121272").Should().BeFalse();
            ImdbHelper.IsValidPersonId("tt1211837").Should().BeFalse("id of a title");
            ImdbHelper.IsValidPersonId("ch0387980").Should().BeFalse("id of a character");
        }

        [TestMethod]
        public void Imdb_Helper_IsValidCharacterId()
        {
            ImdbHelper.IsValidCharacterId("invalid").Should().BeFalse();
            ImdbHelper.IsValidCharacterId(string.Empty).Should().BeFalse();
            ImdbHelper.IsValidCharacterId(null).Should().BeFalse();
            ImdbHelper.IsValidCharacterId("ch0387980").Should().BeTrue("the id of 'Dr. Stephen Strange'");
            ImdbHelper.IsValidCharacterId("hc0387980").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("h0387980").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("0387980").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("ch038798").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("ch 0387980").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("ch-0387980").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("ch 038798").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("ch-038798").Should().BeFalse();
            ImdbHelper.IsValidCharacterId("tt1211837").Should().BeFalse("id of a title");
            ImdbHelper.IsValidCharacterId("nm1212722").Should().BeFalse("id of a person");
        }

        [TestMethod]
        public void Imdb_Helper_GetIdFromUrl()
        {
            ImdbHelper.GetIdFromUrl("http://www.imdb.com/title/tt1211837/?ref_=nv_sr_1").ShouldBeEquivalentTo("tt1211837");
            ImdbHelper.GetIdFromUrl("http://www.imdb.com/title/tt121183/?ref_=nv_sr_1").Should().BeNull();
            ImdbHelper.GetIdFromUrl("http://www.imdb.com/name/nm1212722/?ref_=tt_ov_st_sm").ShouldBeEquivalentTo("nm1212722");
            ImdbHelper.GetIdFromUrl("http://www.imdb.com/name/nm121272/?ref_=tt_ov_st_sm").Should().BeNull();
            ImdbHelper.GetIdFromUrl("http://www.imdb.com/character/ch0387980/?ref_=nm_flmg_act_3").ShouldBeEquivalentTo("ch0387980");
            ImdbHelper.GetIdFromUrl("http://www.imdb.com/character/ch038798/?ref_=nm_flmg_act_3").Should().BeNull();
        }

        [TestMethod]
        public void Imdb_Helper_GetTitleIdFromUrl()
        {
            ImdbHelper.GetTitleIdFromUrl("http://www.imdb.com/title/tt1211837/?ref_=nv_sr_1").ShouldBeEquivalentTo("tt1211837");
            ImdbHelper.GetTitleIdFromUrl("http://www.imdb.com/title/tt121183/?ref_=nv_sr_1").Should().BeNull();
        }

        [TestMethod]
        public void Imdb_Helper_GetPersonIdFromUrl()
        {
            ImdbHelper.GetPersonIdFromUrl("http://www.imdb.com/name/nm1212722/?ref_=tt_ov_st_sm").ShouldBeEquivalentTo("nm1212722");
            ImdbHelper.GetPersonIdFromUrl("http://www.imdb.com/name/nm121272/?ref_=tt_ov_st_sm").Should().BeNull();
        }

        [TestMethod]
        public void Imdb_Helper_GetCharacterIdFromUrl()
        {
            ImdbHelper.GetCharacterIdFromUrl("http://www.imdb.com/character/ch0387980/?ref_=nm_flmg_act_3").ShouldBeEquivalentTo("ch0387980");
            ImdbHelper.GetCharacterIdFromUrl("http://www.imdb.com/character/ch038798/?ref_=nm_flmg_act_3").Should().BeNull();
        }

        [TestMethod]
        public void Imdb_Helper_FormatSearchQuery()
        {
            ImdbHelper.FormatSearchQuery("       ").ShouldBeEquivalentTo("");
            ImdbHelper.FormatSearchQuery("Iñtërnâtiônàlizætiøn").ShouldBeEquivalentTo("internationalizaetion");
            ImdbHelper.FormatSearchQuery("שלום ירושלים").ShouldBeEquivalentTo("shlvm_yrvshlym");
            ImdbHelper.FormatSearchQuery("Strange").ShouldBeEquivalentTo("strange");
            ImdbHelper.FormatSearchQuery(" Strange ").ShouldBeEquivalentTo("strange");
            ImdbHelper.FormatSearchQuery("Dr Strange").ShouldBeEquivalentTo("dr_strange");
            ImdbHelper.FormatSearchQuery("Dr. Strange").ShouldBeEquivalentTo("dr_strange");
            ImdbHelper.FormatSearchQuery("Dr . Strange").ShouldBeEquivalentTo("dr__strange");
            ImdbHelper.FormatSearchQuery("Dr. Strängé").ShouldBeEquivalentTo("dr_strange");
            ImdbHelper.FormatSearchQuery("'Dr. Strange'").ShouldBeEquivalentTo("dr_strange");
            ImdbHelper.FormatSearchQuery("Strange, Dr.").ShouldBeEquivalentTo("strange_dr");
        }
    }
}