using System;
using System.Linq.Expressions;
using System.Reflection;
using Shinobytes.XzaarScript.Compiler.Types;


namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for manipulating type members
    /// </summary>
    public static partial class Members
    {
        /// <summary>
        /// Pops a reference off the evaluation stack and calls the getter of the given property on the object
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to get the value of</param>

        public static XsILGenerator GetProperty(this XsILGenerator generator, PropertyInfo property)
        {
            if (!property.CanRead)
            {
                throw new InvalidOperationException("Cannot read from this property");
            }

            var getMethod = property.GetGetMethod();
            return generator.Call(getMethod);
        }

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the getter of the given property (looked up by name on the given type) on the object
        /// </summary>
        /// <remarks>Will only consider public properties, static or instance</remarks>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>

        public static XsILGenerator GetProperty(this XsILGenerator generator, Type type, string propertyName)
            => generator.GetProperty(GetPropertyInfo(type, propertyName));

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the getter of the given property (looked up by name on the given type) on the object
        /// </summary>
        /// <remarks>Will only consider public properties, static or instance</remarks>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>

        public static XsILGenerator GetProperty<T>(this XsILGenerator generator, string propertyName)
            => generator.GetProperty(typeof(T), propertyName);

        /// <summary>
        /// Calls the getter of the static property represented by the given expression and pushes the value onto the evaluatino stack
        /// </summary>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>

        public static XsILGenerator GetProperty<TProp>(this XsILGenerator generator, Expression<Func<TProp>> expression)
            => generator.GetProperty(GetPropertyInfo(expression));

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the getter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>

        public static XsILGenerator GetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression)
            => generator.GetProperty(GetPropertyInfo(expression));

        /// <summary>
        /// Pops a reference and a value off the evaluation stack and calls the setter of the given property on the object with the value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="property">The property to set</param>

        public static XsILGenerator SetProperty(this XsILGenerator generator, PropertyInfo property)
        {
            if (!property.CanWrite)
            {
                throw new InvalidOperationException("Cannot write to this property");
            }

            var setMethod = property.GetSetMethod();
            return generator.Call(setMethod);
        }

        /// <summary>
        /// Pops a reference and a value off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the value
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the property belongs to</param>
        /// <param name="propertyName">The name of the property on the given <paramref name="type" /></param>

        public static XsILGenerator SetProperty(this XsILGenerator generator, Type type, string propertyName)
            => generator.SetProperty(GetPropertyInfo(type, propertyName));

        /// <summary>
        /// Pops a reference and a value off the evaluation stack and calls the setter of the given property (looked up by name on the given type) on the object with the value
        /// </summary>
        /// <typeparam name="T">The type the property belongs to</typeparam>
        /// <param name="generator"></param>
        /// <param name="propertyName">The name of the property on <typeparamref name="T" /></param>

        public static XsILGenerator SetProperty<T>(this XsILGenerator generator, string propertyName)
            => generator.SetProperty(typeof(T), propertyName);

        /// <summary>
        /// Pops a reference off the evaluation stack and calls the setter of the given property on the object
        /// </summary>
        /// <typeparam name="T">The type the property is on</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="generator"></param>
        /// <param name="expression">An expression that accesses the relevant property</param>

        public static XsILGenerator SetProperty<T, TProp>(this XsILGenerator generator, Expression<Func<T, TProp>> expression)
            => generator.SetProperty(GetPropertyInfo(expression));

        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            if (property == null)
            {
                throw new InvalidOperationException("There is no property called `" + propertyName + "` on the type " + type.Name);
            }

            return property;
        }

        private static PropertyInfo GetPropertyInfo<TProp>(Expression<Func<TProp>> expression)
        {
            var property = (expression.Body as MemberExpression)?.Member as PropertyInfo;

            if (property == null)
            {
                throw new InvalidOperationException("Expression does not represent a property");
            }

            return property;
        }

        private static PropertyInfo GetPropertyInfo<T, TProp>(Expression<Func<T, TProp>> expression)
        {
            var property = (expression.Body as MemberExpression)?.Member as PropertyInfo;

            if (property == null)
            {
                throw new InvalidOperationException("Expression does not represent a property");
            }

            return property;
        }

        /// <summary>
        /// Pops a reference to a delegate instance off the evaluation stack and calls the adder of the given event with it
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="event">The event to add the delegate instance to</param>

        public static XsILGenerator AddToEvent(this XsILGenerator generator, EventInfo @event) => generator.Call(@event.GetAddMethod(true));

        /// <summary>
        /// Pops a reference to a delegate instance off the evaluation stack and call the adder of the event with the given name on the given type
        /// </summary>
        /// <remarks>Will only consider public events, static or instance</remarks>
        /// <param name="generator"></param>
        /// <param name="type">The type the event is on</param>
        /// <param name="eventName">The name of the event</param>

        public static XsILGenerator AddToEvent(this XsILGenerator generator, Type type, string eventName)
            => generator.AddToEvent(GetEventInfo(type, eventName));

        /// <summary>
        /// Pops a reference to a delegate instance off the evaluation stack and call the adder of the event with the given name on the given type
        /// </summary>
        /// <remarks>Will only consider public events, static or instance</remarks>
        /// <typeparam name="T">The type the event is on</typeparam>
        /// <param name="generator"></param>
        /// <param name="eventName">The name of the event</param>

        public static XsILGenerator AddToEvent<T>(this XsILGenerator generator, string eventName)
            => generator.AddToEvent(typeof(T), eventName);

        /// <summary>
        /// Pops a reference to a delegate instance off the evaluation stack and calls the remover of the given event with it
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="event">The event to remove the delegate instance from</param>

        public static XsILGenerator RemoveFromEvent(this XsILGenerator generator, EventInfo @event) => generator.Call(@event.GetRemoveMethod(true));

        /// <summary>
        /// Pops a reference to a delegate instance off the evaluation stack and calls the remover of the event with the given name on the given type
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type">The type the event is on</param>
        /// <param name="eventName">The name of the event</param>

        public static XsILGenerator RemoveFromEvent(this XsILGenerator generator, Type type, string eventName)
            => generator.RemoveFromEvent(GetEventInfo(type, eventName));

        /// <summary>
        /// Pops a reference to a delegate instance off the evaluation stack and calls the remover of the event with the given name on the given type
        /// </summary>
        /// <typeparam name="T">The type the event is on</typeparam>
        /// <param name="generator"></param>
        /// <param name="eventName">The name of the event</param>

        public static XsILGenerator RemoveFromEvent<T>(this XsILGenerator generator, string eventName)
            => generator.RemoveFromEvent(typeof(T), eventName);

        private static EventInfo GetEventInfo(Type type, string eventName)
        {
            var eventInfo = type.GetEvent(eventName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            if (eventInfo == null)
            {
                throw new InvalidOperationException("There is no event called `" + eventName + "` on the type " + type.Name);
            }

            return eventInfo;
        }
    }
}
