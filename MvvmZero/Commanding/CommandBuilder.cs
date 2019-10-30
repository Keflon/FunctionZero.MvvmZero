using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.MvvmZero.Commanding
{
    public class CommandBuilder
    {
        public CommandZeroAsync Build()
        {
            if (_hasBuilt)
                throw new InvalidOperationException("This CommandBuilder has expired. You cannot call Build more than once.");
            _hasBuilt = true;
            return new CommandZeroAsync(_guardList, _execute, _predicate, _getName);
        }

        public CommandBuilder SetExecute(Func<object, Task> execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = execute;
            return this;
        }
        public CommandBuilder SetExecute(Func<Task> execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = (o) => execute();
            return this;
        }
        public CommandBuilder SetExecute(Action execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = (o) => { execute(); return Task.CompletedTask; };
            return this;
        }
        public CommandBuilder SetExecute(Action<object> execute)
        {
            if (_execute != null)
                throw new NotSupportedException("SetExecute cannot be called more than once");
            _execute = (o) => { execute(o); return Task.CompletedTask; };
            return this;
        }

        public CommandBuilder SetCanExecute(Func<object, bool> canExecute)
        {
            if (_predicate != null)
                throw new NotSupportedException("SetCanExecute cannot be called more than once");
            _predicate = canExecute;
            return this;
        }
        public CommandBuilder SetCanExecute(Func<bool> canExecute)
        {
            if (_predicate != null)
                throw new NotSupportedException("SetCanExecute cannot be called more than once");
            _predicate = (o) => canExecute();
            return this;
        }
        public CommandBuilder AddGlobalGuard()
        {
            if (_guardList.Contains(GlobalGuard))
                throw new ArgumentException("Cannot add the global guard to the same command twice");
            _guardList.Add(GlobalGuard);
            return this;
        }
        public static IGuard GlobalGuard { get; } = new BasicGuard();

        public CommandBuilder AddGuard(IGuard guard)
        {
            if (_guardList.Contains(guard))
                throw new ArgumentException("Cannot add the same guard to the same command twice");
            _guardList.Add(guard);
            return this;
        }

        public CommandBuilder SetName(Func<string> getName)
        {
            if (_getName != null)
                throw new NotSupportedException("SetName cannot be called more than once");
            _getName = getName;
            return this;
        }
        public CommandBuilder SetName(string name)
        {
            if (_getName != null)
                throw new NotSupportedException("SetName cannot be called more than once");
            _getName = () => name;
            return this;
        }

        Func<object, Task> _execute;
        Func<object, bool> _predicate;
        IList<IGuard> _guardList;
        Func<string> _getName;
        bool _hasBuilt;

        public CommandBuilder()
        {
            _guardList = new List<IGuard>();
        }
    }
}
