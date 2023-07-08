using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUtour.Commands
{
    abstract class DBCommands<T> where T : class
    {
        private protected ICommand GetCommand = new Command<T>((obj) => 
        {
            
        });
        private protected ICommand AddCommand = new Command<T>((obj) =>
        {

        });
        private protected ICommand DeleteCommand = new Command<T>((obj) =>
        {

        });
        private protected ICommand UpdateCommand = new Command<T>((obj) => 
        { 
            
        });
    }
}
