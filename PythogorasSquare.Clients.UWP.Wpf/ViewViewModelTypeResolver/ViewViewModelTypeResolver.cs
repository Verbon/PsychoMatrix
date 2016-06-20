using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PythogorasSquare.Clients.UWP.Wpf.ViewViewModelTypeResolver
{
    public class ViewViewModelTypeResolver : IViewViewModelTypeResolver
    {
        private static readonly IReadOnlyCollection<string> ViewSuffixes = new[] { "Page", "View", "SettingsFlyout" };
        private const string ViewModelNameSuffix = "ViewModel";

        private readonly IReadOnlyDictionary<Type, Type> _customViewViewModelTypeMappings;
        private readonly Lazy<IDictionary<string, Type>> _uiAssemblyExportedTypes;


        private IDictionary<string, Type> UiAssemblyExportedTypes => _uiAssemblyExportedTypes.Value;


        public ViewViewModelTypeResolver(Assembly uiAssembly)
            : this(uiAssembly, new Dictionary<Type, Type>(0))
        {

        }

        public ViewViewModelTypeResolver(Assembly uiAssembly, IReadOnlyDictionary<Type, Type> customViewViewModelTypeMappings)
        {
            _uiAssemblyExportedTypes = new Lazy<IDictionary<string, Type>>(() => GetUiAssemblyExportedTypes(uiAssembly));
            _customViewViewModelTypeMappings = customViewViewModelTypeMappings;
        }


        public Type GetViewType(string viewTypeName)
            => UiAssemblyExportedTypes[viewTypeName];

        public Type GetViewModelType(Type viewType)
        {
            Type viewModelType;
            if (_customViewViewModelTypeMappings.TryGetValue(viewType, out viewModelType))
            {
                return viewModelType;
            }

            var viewSuffixIndex = ViewSuffixes.Select(viewSuffix => viewType.Name.LastIndexOf(viewSuffix, StringComparison.Ordinal)).Single(index => index != -1);
            var viewNameWithoutSuffix = viewType.Name.Remove(viewSuffixIndex);
            var viewModelName = String.Concat(viewNameWithoutSuffix, ViewModelNameSuffix);
            viewModelType = UiAssemblyExportedTypes[viewModelName];

            return viewModelType;
        }


        private static Dictionary<string, Type> GetUiAssemblyExportedTypes(Assembly uiAssembly)
            => uiAssembly.ExportedTypes.ToDictionary(type => type.Name, StringComparer.Ordinal);
    }
}