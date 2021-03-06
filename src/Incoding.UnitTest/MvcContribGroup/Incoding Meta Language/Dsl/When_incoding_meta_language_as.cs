﻿namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_as
    {
        It should_be_as_html_attribute = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                       .Do().Direct()
                                                       .OnComplete(r => r.Self().Core().Form.Validation.Parse())
                                                       .AsHtmlAttributes(new { @class = "myClass" })
                                                       .Should(dictionary =>
                                                                   {
                                                                       dictionary.Count.ShouldEqual(2);
                                                                       dictionary.ShouldBeKeyValue("class", "myClass");
                                                                       dictionary.ContainsKey("incoding").ShouldBeTrue();
                                                                   });

        It should_be_as_string_attribute = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                         .Do()
                                                         .Direct()
                                                         .OnComplete(r => r.Self().Core().Form.Validation.Parse())
                                                         .AsStringAttributes(new { @class = "myClass" })
                                                         .ShouldEqual(@" class=""myClass""  incoding=""[{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;click incoding&quot;,&quot;onStatus&quot;:0,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableValidationParse&quot;,&quot;data&quot;:{&quot;onBind&quot;:&quot;click incoding&quot;,&quot;onStatus&quot;:4,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}}]"" ");
    }
}