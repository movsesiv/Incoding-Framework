namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    #endregion

    /// <summary>
    ///     Implement provider for StructureMap. http://docs.structuremap.net/
    /// </summary>
    public class StructureMapIoCProvider : IIoCProvider
    {
        #region Fields

        readonly IContainer container;

        #endregion

        #region Constructors

        public StructureMapIoCProvider(Registry registry)
        {
            Guard.NotNull("registry", registry);
            this.container = new Container(registry);
        }

        public StructureMapIoCProvider(Action<Registry> initRegistry)
        {
            Guard.NotNull("initRegistry", initRegistry);

            var registry = new Registry();
            initRegistry(registry);
            this.container = new Container(registry);
        }

        #endregion

        #region IIoCProvider Members

        public TInstance TryGet<TInstance>() where TInstance : class
        {
            return this.container.TryGetInstance<TInstance>();
        }

        public TInstance Get<TInstance>(Type type) where TInstance : class
        {
            return (TInstance)this.container.GetInstance(type);
        }

        public IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance)
        {
            return this.container.GetAllInstances(typeInstance).Cast<TInstance>();
        }

        public TInstance TryGet<TInstance>(Type type) where TInstance : class
        {
            return (TInstance)this.container.TryGetInstance(type);
        }

        public TInstance TryGetByNamed<TInstance>(string named) where TInstance : class
        {
            return this.container.TryGetInstance<TInstance>(named);
        }

        public void Eject<TInstance>()
        {
            this.container.EjectAllInstancesOf<TInstance>();
        }

        public void Forward<TInstance>(TInstance newInstance)
        {
            Eject<TInstance>();
            this.container.Configure(configurationExpression => configurationExpression.For<TInstance>().Use(newInstance));
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (this.container != null)
                this.container.Dispose();
        }

        #endregion
    }
}