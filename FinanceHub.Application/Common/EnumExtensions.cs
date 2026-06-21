using System.ComponentModel;
using System.Reflection;

namespace FinanceHub.Application.Common;

public static class EnumExtensions
{
    /// <summary>
    /// Pega a Description de um item de Enum específico.
    /// </summary>
    public static string GetDescription<T>(this T value) where T : struct, Enum
    {
        FieldInfo field = typeof(T).GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        
        return attribute != null ? attribute.Description : value.ToString();
    }

    /// <summary>
    /// Retorna uma lista com Id, Nome e Description de TODOS os itens do Enum.
    /// Muito útil para mandar para o front carregar combobox/select.
    /// </summary>
    public static List<EnumResponseDto> ToList<T>() where T : struct, Enum
    {
        var list = new List<EnumResponseDto>();

        foreach (T item in Enum.GetValues(typeof(T)))
        {
            list.Add(new EnumResponseDto
            {
                Id = Convert.ToInt32(item),
                Name = item.ToString(),
                Description = item.GetDescription() // Reutiliza o método genérico acima
            });
        }

        return list;
    }
}

public class EnumResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}