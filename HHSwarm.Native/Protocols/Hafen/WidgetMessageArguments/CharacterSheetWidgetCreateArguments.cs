using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <remarks>
    /// @RName("chr")
    /// </remarks>
    class CharacterSheetWidgetCreateArguments
    {
        public enum RESOURCE
        {
            BaseAttributesImage, // 'battr'
            BaseAttributesStrengthImage, // 'str'
            BaseAttributesAgilityImage, // 'agi'
            BaseAttributesIntelligenceImage, // 'int'
            BaseAttributesConstitutionImage, // 'con'
            BaseAttributesPerceptionImage, // 'prc'
            BaseAttributesCharismaImage, // 'csm'
            BaseAttributesDexterityImage, // 'dex'
            BaseAttributesWillImage, // 'wil'
            BaseAttributesPsycheImage, // 'psy'
            BaseAttributesButtonUpImage,
            BaseAttributesButtonDownImage,
            FoodEventPointsImage,
            FoodMeterImage,
            FoodSatuationsImage,
            HungerLevelImage,

            /// <summary>
            /// Food efficacy
            /// </summary>
            GlutMeterImage,

            AbilitiesImage,

            AbilitiesUnarmedImage,
            AbilitiesMeleeImage,
            AbilitiesRangedImage,
            AbilitiesExploreImage,
            AbilitiesStealthImage,
            AbilitiesSewingImage,
            AbilitiesSmithingImage,
            AbilitiesMasonryImage,
            AbilitiesCarpentryImage,
            AbilitiesCookingImage,
            AbilitiesFarmingImage,
            AbilitiesSurviveImage,
            AbilitiesLoreImage,

            AbilitiesButtonAddImage,
            AbilitiesButtonSubtractImage,
            AbilitiesButtonUpImage,
            AbilitiesButtonDownImage,

            StudyReportImage, // 'satr'

            // 'skill'
            LoreAndSkillsImage,
            LoreAndSkillsButtonUpImage,
            LoreAndSkillsButtonDownImage,

            // 'wound'
            HealthAndWoundsImage,
            HealthAndWoundsButtonUpImage,
            HealthAndWoundsButtonDownImage,

            // 'quest'
            QuestLogImage,
            QuestLogButtonUpImage,
            QuestLogButtonDownImage,

            // 'fgt'
            MartialArtsAndCombatSchoolsButtonUpImage,
            MartialArtsAndCombatSchoolsButtonDownImage
        }

        public readonly Dictionary<RESOURCE, string> ResourceName = new Dictionary<RESOURCE, string>();

        //public string BaseAttributesImageResourceName; // 'battr'

        //public string BaseAttributesStrengthImageResourceName; // 'str'
        //public string BaseAttributesAgilityImageResourceName; // 'agi'
        //public string BaseAttributesIntelligenceImageResourceName; // 'int'
        //public string BaseAttributesConstitutionImageResourceName; // 'con'
        //public string BaseAttributesPerceptionImageResourceName; // 'prc'
        //public string BaseAttributesCharismaImageResourceName; // 'csm'
        //public string BaseAttributesDexterityImageResourceName; // 'dex'
        //public string BaseAttributesWillImageResourceName; // 'wil'
        //public string BaseAttributesPsycheImageResourceName; // 'psy'

        //public string BaseAttributesButtonUpImageResourceName;
        //public string BaseAttributesButtonDownImageResourceName;

        //public string FoodEventPointsImageResourceName;
        //public string FoodMeterImageResourceName;
        //public string FoodSatuationsImageResourceName;
        //public string HungerLevelImageResourceName;

        ///// <summary>
        ///// Food efficacy
        ///// </summary>
        //public string GlutMeterImageResourceName;

        //public string AbilitiesImageResourceName;

        //public string AbilitiesUnarmedImageResouceName;
        //public string AbilitiesMeleeImageResouceName;
        //public string AbilitiesRangedImageResouceName;
        //public string AbilitiesExploreImageResouceName;
        //public string AbilitiesStealthImageResouceName;
        //public string AbilitiesSewingImageResouceName;
        //public string AbilitiesSmithingImageResouceName;
        //public string AbilitiesMasonryImageResouceName;
        //public string AbilitiesCarpentryImageResouceName;
        //public string AbilitiesCookingImageResouceName;
        //public string AbilitiesFarmingImageResouceName;
        //public string AbilitiesSurviveImageResouceName;
        //public string AbilitiesLoreImageResouceName;

        //public string AbilitiesButtonAddImageResourceName;
        //public string AbilitiesButtonSubtractImageResourceName;
        //public string AbilitiesButtonUpImageResourceName;
        //public string AbilitiesButtonDownImageResourceName;

        //public string StudyReportImageResourceName; // 'satr'

        //// 'skill'
        //public string LoreAndSkillsImageResourceName; 
        //public string LoreAndSkillsButtonUpImageResourceName;
        //public string LoreAndSkillsButtonDownImageResourceName;

        //// 'wound'
        //public string HealthAndWoundsImageResourceName; 
        //public string HealthAndWoundsButtonUpImageResourceName;
        //public string HealthAndWoundsButtonDownImageResourceName;

        //// 'quest'
        //public string QuestLogImageResourceName; 
        //public string QuestLogButtonUpImageResourceName;
        //public string QuestLogButtonDownImageResourceName;

        //// 'fgt'
        //public string MartialArtsAndCombatSchoolsButtonUpImageResourceName;
        //public string MartialArtsAndCombatSchoolsButtonDownImageResourceName;
    }
}
