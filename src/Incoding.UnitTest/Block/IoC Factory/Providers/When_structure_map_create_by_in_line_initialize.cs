namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.Utilities;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(StructureMapIoCProvider))]
    public class When_structure_map_create_by_in_line_initialize : Context_IoC_Provider
    {
        Establish establish = () =>
                                  {
                                      ioCProvider = new StructureMapIoCProvider(registry =>
                                                                                    {
                                                                                        registry.For<IEmailSender>().Use(defaultInstance);
                                                                                        registry.For<ILogger>().Use<ConsoleLogger>().Named(consoleNameInstance);

                                                                                        registry.Scan(scanner =>
                                                                                                          {
                                                                                                              scanner.Assembly(typeof(IFakePlugIn).Assembly);
                                                                                                              scanner.AddAllTypesOf<IFakePlugIn>();
                                                                                                          });
                                                                                    });
                                  };

        Behaves_like<Behaviors_get_ioc_provider> verify_get_and_try_get_operation;

        Behaves_like<Behaviors_expected_exception_ioc_provider> verify_expected_exception;

        Behaves_like<Behaviors_forward_ioc_provider> verify_forward;

        Behaves_like<Behaviors_eject_ioc_provider> verify_eject;
    }
}