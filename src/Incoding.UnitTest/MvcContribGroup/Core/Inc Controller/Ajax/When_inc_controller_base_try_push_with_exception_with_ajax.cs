﻿namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Specialized;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncControllerBase))]
    public class When_inc_controller_base_try_push_with_exception_with_ajax : Context_inc_controller_base
    {
        #region Estabilish value

        static IncFakeException exception;

        #endregion

        Establish establish = () =>
                                  {
                                      httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });
                                      dispatcher.StubPushAsThrow(new FakeCommand(), new IncFakeException());
                                  };

        Because of = () => { exception = Catch.Exception(() => controller.Push(new FakeCommand())) as IncFakeException; };

        It should_be_exception = () => exception.ShouldNotBeNull();
    }
}