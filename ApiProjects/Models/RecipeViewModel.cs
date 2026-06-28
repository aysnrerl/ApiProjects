using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiProjects.Models
{
    public class RecipeViewModel
    {
        [JsonPropertyName("meals")]
        public List<MealDbItem> meals { get; set; }
    }

    public class MealDbItem
    {
        [JsonPropertyName("idMeal")]
        public string idMeal { get; set; }

        [JsonPropertyName("strMeal")]
        public string strMeal { get; set; }

        [JsonPropertyName("strCategory")]
        public string strCategory { get; set; }

        [JsonPropertyName("strArea")]
        public string strArea { get; set; }

        [JsonPropertyName("strInstructions")]
        public string strInstructions { get; set; }

        [JsonPropertyName("strMealThumb")]
        public string strMealThumb { get; set; }
    }
}