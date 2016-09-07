using System.Collections.Generic;
using NuimoHub.Interfaces;

namespace NuimoHelpers
{
    public static class ApplicationCaroussel
    {
        public static LinkedListNode<INuimoApp> NextOrFirst(this LinkedListNode<INuimoApp> current)
        {
            if (current.Next == null)
                return current.List.First;
            return current.Next;
        }

        public static LinkedListNode<INuimoApp> PreviousOrLast(this LinkedListNode<INuimoApp> current)
        {
            if (current.Previous == null)
                return current.List.Last;
            return current.Previous;
        }
    }
}