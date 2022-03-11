using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using static Xamarin.Forms.BindableProperty;

namespace TestProject.UI.Extensions
{
    public static class BindableObjectExtensions
    {
        private static readonly Dictionary<Type, Dictionary<string, BindableProperty>> cachedBindableProperties = new Dictionary<Type, Dictionary<string, BindableProperty>>();

        public delegate void BindablePropertyChangedHandler<TProperty, TParent>(TParent parent, TProperty oldValue, TProperty newValue);

        public delegate void BindablePropertyChangedHandler<TProperty>(TProperty oldValue, TProperty newValue);

        public delegate bool ValidateValuePredicate<TProperty>(TProperty value);

        public static T GetProperty<T>(this BindableObject bindableObject, [CallerMemberName] string propertyName = null)
        {
            var propertyFieldName = $"{propertyName}Property";
            var propertyType = bindableObject.GetType();
            var propertyField = GetBindablePropertyField(propertyType, propertyFieldName);

            var propertyValue = (T)bindableObject.GetValue(propertyField);
            return propertyValue;
        }

        public static void SetProperty<T>(
            this BindableObject bindableObject,
            T value,
            [CallerMemberName] string propertyName = null)
        {
            var propertyFieldName = $"{propertyName}Property";
            var propertyType = bindableObject.GetType();
            var propertyField = GetBindablePropertyField(propertyType, propertyFieldName);

            bindableObject.SetValue(propertyField, value);
        }

        private static BindableProperty GetBindablePropertyField(Type bindableObjectType, string bindablePropertyFieldName)
        {
            if (!cachedBindableProperties.ContainsKey(bindableObjectType))
            {
                cachedBindableProperties[bindableObjectType] = new Dictionary<string, BindableProperty>();
            }

            var typePropertiesDictionary = cachedBindableProperties[bindableObjectType];
            if (!typePropertiesDictionary.ContainsKey(bindablePropertyFieldName))
            {
                var propertyFieldBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
                var propertyFieldInfo = bindableObjectType.GetField(bindablePropertyFieldName, propertyFieldBindingFlags);
                var propertyField = (BindableProperty)propertyFieldInfo.GetValue(null);

                typePropertiesDictionary[bindablePropertyFieldName] = propertyField;
            }

            return typePropertiesDictionary[bindablePropertyFieldName];
        }

        public static void SetBasis(this BindableObject bindableObject, FlexBasis basis) =>
            FlexLayout.SetBasis(bindableObject, basis);

        public static FlexBasis GetBasis(this BindableObject bindableObject) =>
            FlexLayout.GetBasis(bindableObject);

        public static BindableProperty Create<TProperty, TParent>(string propertyName,
                                                                  Func<TParent, BindablePropertyChangedHandler<TProperty>> onPropertyChangedHandlerGetter = null,
                                                                  TProperty defaultValue = default,
                                                                  BindingMode defaultBindingMode = BindingMode.OneWay) where TParent : BindableObject
        {

            void OnPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
            {
                var parent = (TParent)bindableObject;
                var parentOnPropertyChangedHandler = onPropertyChangedHandlerGetter?.Invoke(parent);
                var castedOldValue = (TProperty)oldValue;
                var castedNewValue = (TProperty)newValue;

                parentOnPropertyChangedHandler?.Invoke(castedOldValue, castedNewValue);
            }

            var hasOnPropertyChangedHandler = onPropertyChangedHandlerGetter != null;
            var onPropertyChangedHandler = hasOnPropertyChangedHandler ? new BindingPropertyChangedDelegate(OnPropertyChanged) : null;

            var prop = BindableProperty.Create(propertyName,
                                               typeof(TProperty),
                                               typeof(TParent),
                                               defaultValue,
                                               defaultBindingMode,
                                               null,
                                               onPropertyChangedHandler);
            return prop;
        }

        public static BindableProperty CreateAttached<TProperty, TParent>(string propertyName,
                                                                          BindablePropertyChangedHandler<TProperty, TParent> onPropertyChangedHandlerGetter = null,
                                                                          TProperty defaultValue = default,
                                                                          BindingMode defaultBindingMode = BindingMode.OneWay) where TParent : BindableObject
        {
            void OnPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
            {
                var parent = (TParent)bindableObject;
                var castedOldValue = (TProperty)oldValue;
                var castedNewValue = (TProperty)newValue;

                onPropertyChangedHandlerGetter?.Invoke(parent, castedOldValue, castedNewValue);
            }

            var hasOnPropertyChangedHandler = onPropertyChangedHandlerGetter != null;
            var onPropertyChangedHandler = hasOnPropertyChangedHandler ? new BindingPropertyChangedDelegate(OnPropertyChanged) : null;

            var prop = BindableProperty.CreateAttached(propertyName,
                                                       typeof(TProperty),
                                                       typeof(TParent),
                                                       defaultValue,
                                                       defaultBindingMode,
                                                       propertyChanged: onPropertyChangedHandler);
            return prop;
        }
    }
}
