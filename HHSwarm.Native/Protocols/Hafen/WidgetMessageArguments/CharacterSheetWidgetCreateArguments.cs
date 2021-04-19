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
    class CharacterSheetWidgetCreateArguments : ResourceNamesWidgetCreateArguments<CharacterSheetWidgetCreateArguments.RESOURCE_NAME>
    {
        public enum RESOURCE_NAME
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

            AbilitiesButtonAddUpImage,
            AbilitiesButtonAddDownImage,
            AbilitiesButtonSubtractUpImage,
            AbilitiesButtonSubtractDownImage,
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
    }
}
