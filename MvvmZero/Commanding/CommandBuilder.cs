using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.MvvmZero.Commanding
{
#if false
    public class CommandBuilder
    {

        public CommandZeroAsync Build()
        {
            if (_executeFuncList.Count == 1)
                return new CommandZeroAsync(_guard, _executeFuncList[0], _predicate);

            return new CommandZeroAsync(_guard, async (o) => { foreach (var func in _executeFuncList) await func(o); }, _predicate);

        }

        public CommandBuilder AddExecute(Func<object, Task> execute)
        {
            _executeFuncList.Add(execute);
            return this;
        }
        public CommandBuilder AddExecute(Func<Task> execute)
        {
            _executeFuncList.Add((o) => execute());
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

        public CommandBuilder SetGuard(IGuard guard)
        {
            if (_guard != null)
                throw new NotSupportedException("SetGuard cannot be called more than once");
            _guard = guard;
            return this;
        }

        public CommandBuilder AddGuard(IGuard guard)
        {
            throw new NotImplementedException("This requires a change to IGuard.IsGuardRaised from bool to a reference count.");
        }

        IList<Func<object, Task>> _executeFuncList;
        Func<object, bool> _predicate;
        IGuard _guard;

        public CommandBuilder()
        {
            _executeFuncList = new List<Func<object, Task>>();
            _predicate = null;
            _guard = null;
        }
    }
#endif
}
