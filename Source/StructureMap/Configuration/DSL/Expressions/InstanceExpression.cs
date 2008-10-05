﻿using System;
using StructureMap.Pipeline;

namespace StructureMap.Configuration.DSL.Expressions
{
    public interface IsExpression<T>
    {
        InstanceExpression<T> Is { get; }
        void IsThis(Instance instance);
        LiteralInstance IsThis(T obj);
    }

    public class GenericIsExpression
    {
        private readonly Action<Instance> _action;

        public GenericIsExpression(Action<Instance> action)
        {
            _action = action;
        }


        public ConfiguredInstance Is(Type concreteType)
        {
            var instance = new ConfiguredInstance(concreteType);
            _action(instance);

            return instance;
        }
    }

    public class InstanceExpression<T> : IsExpression<T>
    {
        private readonly Action<Instance> _action;

        internal InstanceExpression(Action<Instance> action)
        {
            _action = action;
        }

        public void Instance(Instance instance)
        {
            _action(instance);
        }

        private T returnInstance<T>(T instance) where T : Instance
        {
            Instance(instance);
            return instance;
        }

        public SmartInstance<PLUGGEDTYPE> OfConcreteType<PLUGGEDTYPE>() where PLUGGEDTYPE : T
        {
            return returnInstance(new SmartInstance<PLUGGEDTYPE>());
        }

        public ConfiguredInstance OfConcreteType(Type type)
        {
            return returnInstance(new ConfiguredInstance(type));
        }

        public LiteralInstance Object(T theObject)
        {
            return returnInstance(new LiteralInstance(theObject));
        }

        InstanceExpression<T> IsExpression<T>.Is
        {
            get { return this; }
        }

        public void IsThis(Instance instance)
        {
            returnInstance(instance);
        }

        public LiteralInstance IsThis(T obj)
        {
            return returnInstance(new LiteralInstance(obj));
        }

        public ReferencedInstance References(string key)
        {
            return returnInstance(new ReferencedInstance(key));
        }

        public DefaultInstance TheDefault()
        {
            return returnInstance(new DefaultInstance());
        }

        public ConstructorInstance<T> ConstructedBy(Func<T> func)
        {
            return returnInstance(new ConstructorInstance<T>(func));
        }

        public ConstructorInstance<T> ConstructedBy(Func<IContext, T> func)
        {
            return returnInstance(new ConstructorInstance<T>(func));
        }

        public PrototypeInstance PrototypeOf(T template)
        {
            return returnInstance(new PrototypeInstance((ICloneable) template));
        }

        public SerializedInstance SerializedCopyOf(T template)
        {
            return returnInstance(new SerializedInstance(template));
        }

        public UserControlInstance LoadControlFrom(string url)
        {
            return returnInstance(new UserControlInstance(url));
        }
    }
}
