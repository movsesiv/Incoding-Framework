namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_not_empty_query
    {
        #region Fake classes

        public class FakeEntity : IEntity
        {
            #region IEntity Members

            public object Id { get; private set; }

            #endregion
        }

        class FakeMockMessage : QueryBase<List<FakeEntity>>
        {
            #region Override

            protected override List<FakeEntity> ExecuteResult()
            {
                return Repository.Query<FakeEntity>().ToList();
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockMessage<FakeMockMessage, List<FakeEntity>> mockMessage;

        #endregion

        Establish establish = () =>
                                  {
                                      mockMessage = MockQuery<FakeMockMessage, List<FakeEntity>>
                                              .When(new FakeMockMessage())
                                              .StubNotEmptyQuery<FakeEntity>(countEntity: 10);
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_not_empty_result = () => mockMessage.ShouldBeIsResult(list => list.Count.ShouldEqual(10));
    }
}