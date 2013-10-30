﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace ErlangEditor.ViewModel
{
    /// <summary>
    /// WPF MVVC设计模式ViewMode基本功能类
    /// </summary>
    public abstract class ViewModelBase : DependencyObject,INotifyPropertyChanged, IDisposable
    {
        #region Public Properties
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; protected set; }
        #endregion
        #region Constructor
        /// <summary>
        /// 实例化一个ViewModelBase对象
        /// </summary>
        protected ViewModelBase()
        {
        }
        #endregion
        #region INotifyPropertyChanged Members
        /// <summary>
        /// 触发属性发生变更事件
        /// </summary>
        /// <typeparam name="T">泛型标记，会匹配函数返回类型，不必手动填写</typeparam>
        /// <param name="action">以函数表达式方式传入属性名称，表达式如下即可：()=>YourViewModelProperty</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            var propertyName = GetPropertyName(action);
            OnPropertyChanged(propertyName);
        }
        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return propertyName;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region IDisposable Members
        public void Dispose()
        {
            this.OnDispose();
        }
        /// <summary>
        /// 若支持IDisposable，请重写此方法，当被调用Dispose时会执行此方法。
        /// </summary>
        protected virtual void OnDispose()
        {
        }
        #endregion
    }
}
