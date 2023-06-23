using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.Timeliner;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace EXNAVIS
{
    // Plugin name,Developer ID or GUID,Display name for the Plugin in the Ribbon
    [PluginAttribute("4DS", "Navisworks", DisplayName = "4DS")]
    public class SequenceChecker : AddInPlugin
    {
        // to collect all errorMessages
        private readonly List<string> errorMessages = new List<string>();
        public override int Execute(params string[] parameters)
        {
            // Get the active Document object
            Document Doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            // Get the 4D Timeline data
            DocumentTimeliner timeLinerDoc = TimelinerDocumentExtensions.GetTimeliner(Doc);
            // Create a dictionary to store the start date and elevation of each model item
            Dictionary<string, List<(ModelItem, DateTime, double)>> itemDataDict = new Dictionary<string, List<(ModelItem, DateTime, double)>>();
            // Loop through the tasks and get their children
            foreach (TimelinerTask RTask in timeLinerDoc.Tasks)
            {
                // Get the children of the task
                // Loop through the children and print their display names
                foreach (TimelinerTask childTask in RTask.Children)
                {
                    TimelinerSelection timeLinerSelection = childTask.Selection;

                    if (timeLinerSelection != null)
                    {
                        ModelItemCollection modelItems = timeLinerSelection.GetSelectedItems(Doc); // get one child
                        // Get the Display ID of the current TimelinerTask
                        string displayId = childTask.DisplayId;
                        foreach (ModelItem item in modelItems)   // get data/properties of that child 
                        {

                            // Get the start date and elevation of the model item
                            PropertyCategory itemStartDateCPro = item.PropertyCategories.FindCategoryByDisplayName("TimeLiner");
                            DataProperty itemStartDatePro = itemStartDateCPro.Properties.FindPropertyByDisplayName("Attached to Task Start (Planned):1");
                            VariantData vitemStartDate = itemStartDatePro.Value;
                            DateTime itemStartDate = vitemStartDate.ToDateTime();

                            /////////////// if i didn't find that element , show message , but continue***********
                            PropertyCategory itemElevationCPro = item.PropertyCategories.FindCategoryByDisplayName("Element")
                                //?? item.PropertyCategories.FindCategoryByDisplayName("Base Level")
                                //?? item.PropertyCategories.FindCategoryByDisplayName("Reference Level")
                                //?? item.PropertyCategories.FindCategoryByDisplayName("Level")
                                //?? item.PropertyCategories.FindCategoryByDisplayName("Base Constraint")
                                ;
                            ///////////// add more conditions didn't find parameter named element or level 
                            DataProperty itemElevationPro = itemElevationCPro.Properties.FindPropertyByDisplayName("Elevation at Bottom")
                            ?? itemElevationCPro.Properties.FindPropertyByDisplayName("End Level Offset")
                            ?? itemElevationCPro.Properties.FindPropertyByDisplayName("End Extension")
                            ;

                            // Retrieve the level elevation
                            PropertyCategory levelElevationCPro = item.PropertyCategories.FindCategoryByDisplayName("Base Level");
                            DataProperty levelElevationPro = levelElevationCPro?.Properties.FindPropertyByDisplayName("Elevation");
                            VariantData vLevelElevation = levelElevationPro?.Value;

                           


                            //*************************************************
                            // itemElevationCPro.Properties.FindPropertyByDisplayName("Elevation")
                            // Retrieve the elevation value
                            VariantData vitemElevation = itemElevationPro?.Value;
                            double itemElevation = vitemElevation?.ToDoubleLength() 
                                ?? vLevelElevation?.ToDoubleLength() 
                                ?? 0;





                            // DataProperty itemTaskName = itemStartDateCPro.Properties.FindPropertyByDisplayName("Attached to Task: 1");
                            // string TaskName = itemTaskName.ToString();
                            if (!itemDataDict.ContainsKey(displayId)) // check if dictionary does not contain the displayId
                            {
                                itemDataDict[displayId] = new List<(ModelItem, DateTime, double)>();
                                // This line creates a new empty List<(ModelItem, DateTime, double)>
                                // and assigns it as the value for the displayId key in the itemDataDict dictionary.
                                // This list will store the data related to the model items associated with the current displayId
                            }
                            itemDataDict[displayId].Add((item, itemStartDate, itemElevation));
                        }
                        foreach (ModelItem item in modelItems)
                        {
                            // Get the start date and elevation of the model item
                            // ...

                            PropertyCategory itemElevationCPro = item.PropertyCategories.FindCategoryByDisplayName("Element");

                            // Check if the itemElevationCPro is null, indicating that the desired category was not found
                            if (itemElevationCPro == null)
                            {
                                // Display a message indicating that the category was not found
                                string errorMessage = $"Category 'Element' not found for item '{item.DisplayName}'";
                                NotifyUser(errorMessage);

                                // Continue to the next iteration
                                continue;
                            }

                            // Retrieve the data properties based on additional conditions
                            DataProperty itemElevationPro = itemElevationCPro.Properties.FindPropertyByDisplayName("Elevation at Bottom")
                                ?? itemElevationCPro.Properties.FindPropertyByDisplayName("Base Offset")
                                ?? itemElevationCPro.Properties.FindPropertyByDisplayName("End Level Offset")
                                ?? itemElevationCPro.Properties.FindPropertyByDisplayName("End Extension");

                            // Check if the itemElevationPro is null, indicating that none of the desired properties were found
                            if (itemElevationPro == null)
                            {
                                // Display a message indicating that none of the properties were found
                                string errorMessage = $"No suitable properties found for item '{item.DisplayName}'";
                                NotifyUser(errorMessage);

                                // Continue to the next iteration
                                continue;
                            }

                        }
                        /*
                        foreach (ModelItem item in modelItems)
                        {
                            // Get the start date and elevation of the model item
                            // ...

                            PropertyCategory itemElevationCPro = item.PropertyCategories.FindCategoryByDisplayName("Element");

                            // Check if the itemElevationCPro is null, indicating that the desired category was not found
                            if (itemElevationCPro == null)
                            {
                                // Display a message indicating that the category was not found
                                string errorMessage = $"Category 'Element' not found for item '{item.DisplayName}'";
                                NotifyUser(errorMessage);

                                // Continue to the next iteration
                                continue;
                            }

                            // Retrieve the data properties based on additional conditions
                            DataProperty itemElevationPro = itemElevationCPro.Properties.FindPropertyByDisplayName("Elevation at Bottom")
                                ?? itemElevationCPro.Properties.FindPropertyByDisplayName("Base Offset")
                                ?? itemElevationCPro.Properties.FindPropertyByDisplayName("End Level Offset")
                                ?? itemElevationCPro.Properties.FindPropertyByDisplayName("End Extension");

                            // Check if the itemElevationPro is null, indicating that none of the desired properties were found
                            if (itemElevationPro == null)
                            {
                                // Display a message indicating that none of the properties were found
                                string errorMessage = $"No suitable properties found for item '{item.DisplayName}'";
                                NotifyUser(errorMessage);

                                // Continue to the next iteration
                                continue;
                            }

                            // Retrieve the elevation value
                            VariantData vitemElevation = itemElevationPro.Value;
                            double itemElevation = vitemElevation?.ToDoubleLength() ?? 0;

                            // Check if the item doesn't have a level (elevation is zero and not on Level 0)
                            if (itemElevation == 0 && itemElevationPro.DisplayName != "Level 0")
                            {
                                // Display a message indicating that the item is not on any level
                                string errorMessage = $"The item '{item.DisplayName}' is not on any level";
                                NotifyUser(errorMessage);

                                // Continue to the next iteration
                                continue;
                            }


                        }
                     */
                    }
                }
            }
            // Sort the items in each displayID list/group by elevation and start date
            foreach (var kvp1 in itemDataDict)//*****************************************
            {
                foreach (var kvp2 in itemDataDict) // like check if nothing have same elevation , start , end ( overlap) ...i.e same elements not have two displayid
                {
                    if (kvp1.Key != kvp2.Key) // compare different activities that have different display id ( not repeated id) 
                    {
                        List<(ModelItem, DateTime, double)> itemDataList1 = kvp1.Value;
                        List<(ModelItem, DateTime, double)> itemDataList2 = kvp2.Value;
                        int i = 0;
                        int j = 0;
                        while (i < itemDataList1.Count && j < itemDataList2.Count)
                        {
                            var currentItem = itemDataList1[i];
                            var nextItem = itemDataList2[j];

                            if (nextItem.Item3 < currentItem.Item3 && nextItem.Item2 > currentItem.Item2)
                            {
                                string message = $"The item '{currentItem.Item1.DisplayName}' with '{currentItem.Item3}' in Display ID '{kvp1.Key}' is Above the item '{nextItem.Item1.DisplayName}' with '{nextItem.Item3}' in Display ID '{kvp2.Key}', You should check the Time Schedule.";
                                // Check if the error message already exists in the list
                                if (!errorMessages.Contains(message))
                                {
                                    errorMessages.Add(message);
                                }
                            }
                            if (nextItem.Item3 > currentItem.Item3 && nextItem.Item2 < currentItem.Item2)
                            {
                                string message = $"The item '{currentItem.Item1.DisplayName}'with '{currentItem.Item3}'in Display ID '{kvp1.Key}' is below the item '{nextItem.Item1.DisplayName}' with '{nextItem.Item3}'in Display ID '{kvp2.Key}', You should check the Time Schedule.";
                                // Check if the error message already exists in the list
                                if (!errorMessages.Contains(message))
                                {
                                    errorMessages.Add(message);
                                }

                            }

                            if (currentItem.Item3 <= nextItem.Item3 && currentItem.Item2 <= nextItem.Item2)
                            {
                                i++;
                            }
                            else
                            {
                                j++;
                            }
                        }
                    }
                }
            }
            if (errorMessages.Count > 0)
            {
                string message = string.Join("\n", errorMessages);
                NotifyUser(message);
            }
            else
            {
                string message = "every element is set correctly with the time Schedule";
                NotifyUser(message);
            }
            return 0;
        }
        private void NotifyUser(string message)
        {
            // Display a message box to notify the user
            Task.Run(() => System.Windows.MessageBox.Show(message));
        }
    }
}