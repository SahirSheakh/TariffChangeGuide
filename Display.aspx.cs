using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.EnterpriseServices;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TariffChangeGuide
{
    public partial class Display : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void RadioTM_CheckedChanged(object sender, EventArgs e)
        {

            if (RadioTM.Checked)
            {
                if (RadioResidential.Checked)
                {
                    OutputLiteral.Text = "Tariff change guide rules:<br />" +
                        "<ul>" +
                        "<li>If one single tariff requesting to change to another single new tariff  - Admin Tariff Change (TN)</li>" +
                        "<li>If one single tariff (with an additional solar) requesting to change to another single new tariff (with an additional solar) - Admin Tariff Change (TN)</li>" +
                        "<li>If one single tariff (with an additional off-peak) requesting to change to another single new tariff (with an additional off-peak with no change) - Admin Tariff Change (TN)</li>" +
                        "<li>If changing TAS61 to TAS62 - Meter Reconfiguration (TM)</li>" +
                        "<li>If a customer has TAS61 and requests to change to TOU, TAS61 will be changed to TAS62</li>" +
                        "<li>You cannot change from TAS62 to TAS61</li>" +
                        "<li>Check if an additional tariff is being added or a tariff is being removed - EWR</li>" +
                        "<li>TAS63 is the network tariff code for TAS62</li>" +
                        "</ul>";

                }
                           

            }
            else if (RadioBM.Checked)
            {
                if (RadioResidential.Checked)
                {
                    OutputLiteral.Text = "";
                    OutputLabel.Text = "";
                }
            }
            else if (RadioYM.Checked)
            {
                if (RadioResidential.Checked)
                {
                    OutputLiteral.Text = "";
                    OutputLabel.Text = "";
                }
            }
           
        }
      
        //Basic meter tariff process

        Dictionary<(string, string), string> basicmetertariffProcesses = new Dictionary<(string, string), string>
        {
            { ("TAS22", "TAS31"), "Admin" },
            { ("TAS22", "TAS31, TAS41"), "EWR" },
            { ("TAS22", "TAS93"), "Site Visit" },
            { ("TAS22", "TAS94"), "Site Visit" },
            { ("TAS22", "TAS93, TAS62"), "EWR" },
            { ("TAS22, TAS22", "TAS31"), "EWR" },
            { ("TAS22, TAS22", "TAS31, TAS41"), "Admin" },
            { ("TAS22, TAS22", "TAS93"), "Site Visit" },
            { ("TAS22, TAS22", "TAS94"), "Site Visit" },
            { ("TAS22, TAS22", "TAS93, TAS62"), "Site Visit" },
            { ("TAS31", "TAS22"), "Admin" },
            { ("TAS31", "TAS93"), "Site Visit" },
            { ("TAS31", "TAS94"), "Site Visit" },
            { ("TAS31", "TAS22, TAS41"), "EWR" },
            { ("TAS31, TAS41", "TAS22"), "EWR" },
            { ("TAS31, TAS41", "TAS93"), "Site Visit" },
            { ("TAS31, TAS41", "TAS94"), "Site Visit" },
            { ("TAS31, TAS41", "TAS22, TAS41"), "Admin" },
            { ("TAS31, TAS41", "TAS31, TAS62"), "Site Visit" },
            { ("TAS41", "TAS62"), "Site Visit" }
        };

        //TasMetering tariff process
        Dictionary<(string, string), string> TasMetertariffProcesses = new Dictionary<(string, string), string>
        {
                { ("TAS31", "TAS93"), "Admin Tariff Change (TN)" },
                { ("TAS31, TASX4I", "TAS93, TASX4I"), "Admin Tariff Change (TN)" },
                { ("TAS31, TAS41", "TAS93"), "Admin Tariff Change (TN)" },
                { ("TAS31, TAS41", "TAS31"), "Additional tariff - Admin Tariff Change (TN) & Meter Reconfiguration (Y)" },
                { ("TAS31, TAS61", "TAS31, TAS63"), "Meter Reconfiguration (TM) and EWR may be required" },
                { ("TAS31, TAS61", "TAS93, TAS63"), "Meter Reconfiguration (TM)" },
                { ("TAS31, TAS63", "TAS93 & TAS63"), "Admin Tariff Change (TN)" },
                { ("TAS31, TAS41, TASX4I", "TAS93, TASX4I"), "Meter Reconfiguration (TM)" },
                { ("TAS31, TAS41, TAS61", "TAS93, TAS63"), "Meter Reconfiguration (TM)" },
                { ("TAS31, TAS61, TASX4I", "TAS31, TAS63, TASX4I"), "Meter Reconfiguration (TM) and EWR may be required" },
                { ("TAS31, TAS61, TASX4I", "TAS93, TAS63, TASX4I"), "Meter Reconfiguration (TM)" },
                { ("TAS31, TAS63, TASX4I", "TAS93, TAS63, TASX4I"), "Admin Tariff Change (TN)" },
                { ("TAS31, TAS41, TAS61, TASX4I", "TAS31, TAS41, TAS63, TASX4I"), "Meter Reconfiguration (TM) and EWR may be required" },
                { ("TAS31, TAS41, TAS61, TASX4I", "TAS93, TAS63, TASX4I"), "Meter Reconfiguration (TM)" },
                { ("TAS31, TAS41, TAS63, TASX4I", "TAS93, TAS63, TASX4I"), "Meter Reconfiguration (TM)" },
                { ("TAS93", "TAS31"), "Admin Tariff Change (TN)" },
                { ("TAS93", "TAS31, TAS41"), "Meter Reconfiguration (TM) and EWR may be required" },
                { ("TAS93, TAS63", "TAS31, TAS63"), "Meter Reconfiguration (TM) and EWR may be required" },
                { ("TAS93, TAS63", "TAS31, TAS41, TAS63"), "Check if an additional tariff is being added or a tariff is being removed - EWR\nCheck if 1 meter - Meter Reconfiguration (TM) and EWR may be required\nCheck if 2 meters - Meter Reconfiguration (TM)" },
                { ("TAS93, TASX4I", "TAS31, TASX4I"), "Admin Tariff Change (TN)" },
                { ("TAS93, TASX4I", "TAS31, TAS41, TASX4I"), "Meter Reconfiguration (TM) and EWR may be required" },
                { ("TAS93, TAS63, TASX4I", "TAS31, TAS62, TASX4I"), "Meter Reconfiguration (TM) and EWR may be required" },
                { ("TAS93, TAS63, TASX4I", "TAS31, TAS41, TAS63, TASX4I"), "Meter Reconfiguration (TM) and EWR may be required" },
                // Add more entries based on the table
            

        };

       

        protected void Submit_Click(object sender, EventArgs e)
        {

            // Reset the OutputLiteral.Text to an empty string
            OutputLiteral.Text = "";
            OutputLabel.Text = "";



            // condition for Basic Meter and Residential

            if (RadioBM.Checked)
            {
                if (RadioResidential.Checked)
                {
                    string from = TBFrom.Text.ToUpper();
                    string to = TBTo.Text.ToUpper();

                    if (basicmetertariffProcesses.TryGetValue((from, to), out string process))
                    {

                        OutputLabel.Text = process;

                        if (process.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {





                            // Build the text content with line breaks
                            OutputLiteral.Text += "FIRST PRIORITY IS METER EXCHANGE REQUEST" +

                                                    "<br />" +
                                                    "OR<br />" +

                                                "To make a change for the following tariffs follow the process for ADMIN<br />" +
                                                "Tariff change <a href='#' onclick='openFeeLink(); return false;'>fee</a>.<br />" +
                                                "<br />" +
                                                 "<br />" +
                                             "NOTE:<br />" +
                                              "<br />" +
                                 "<ul>" +
                                "<li>An administrative tariff change must meet further requirements:</li>" +
                                "<li>There must be consumption on all meters. If no consumption, an EWR is required to ensure the wiring/circuits have been confirmed as safe by an electrical contractor.</li>" +
                                "<li>Can only be changed from a true read. If the previous read was estimated, then the NSRD will be used. Advise customer clear access must be obtained for NSRD in order to complete request.</li>" +
                                "<li>An administrative tariff change requested within 10 business days of the NSRD can only be changed from the NSRD and not the previous read.</li>" +
                                "<li>For a customer moving in to a premise, the administrative tariff change will be active from the move in date.</li>" +
                                "<li>If a customer has TAS22 & TAS22, it is recommended an EWR is submitted by an electrical contractor. This helps to ensure TAS41 is correctly assigned to heating / hot water appliances. If an administrative tariff change is possible once the EWR is received, it can still be processed in this way. If the customer does not wish to submit an EWR, then we can proceed with their request at their own risk.</li>" +
                                 "</ul>";

                        }
                        else if (process.Equals("EWR", StringComparison.OrdinalIgnoreCase))
                        {
                            OutputLiteral.Text += "FIRST PRIORITY IS METER EXCHANGE REQUEST" +

                                "<br />" +
                                "OR<br />" +
                                "<ul>" +
                                "<li>​EWR: Meter exchange can only be arranged once an EWR has been received.</li>" +
                                "<li>No fee applies.</li>" +
                                "</ul>";
                        }
                        else if (process.Equals("Site Visit", StringComparison.OrdinalIgnoreCase))
                        {
                            OutputLiteral.Text += "FIRST PRIORITY IS METER EXCHANGE REQUEST" +

                               "<br />" +
                               "OR<br />" +
                               "<ul>" +
                                "<li>​Site visit: Meter exchange can be arranged without an EWR. Transfer to Customer Metering queue.</li>" +
                                "<li>No fee applies.</li>" +
                                "</ul>";
                        }
                        else
                        {
                            OutputLiteral.Text += "NOT RESULT FOUND";
                        }
                    }

                }
            }

            //Conditions for TasMetering and Residentital
            if (RadioTM.Checked)
            {
                if (RadioResidential.Checked)
                {
                    string from = TBFrom.Text.ToUpper();
                    string to = TBTo.Text.ToUpper();

                    if (TasMetertariffProcesses.TryGetValue((from, to), out string process))
                    {


                        OutputLabel.Text = process;

                        if (process.Equals("Admin Tariff Change (TN)", StringComparison.OrdinalIgnoreCase))
                        {





                            // Build the text content with line breaks
                            OutputLiteral.Text += "Admin tariff change only:" +

                                                    "<br />" +
                                                    "<br />" +

                                                "No site visit is required.<br />" +
                                                "A TasNetworks Admin Tariff change <a href='#' onclick='openFeeLink(); return false;'>fee</a> applies.<br />"+
                                                "Panviva <a href='#' onclick='openFeeLink(); return false;'>Tas Metering fees</a>";


                        }
                        else if (process.Equals("Additional tariff - Admin Tariff Change (TN) & Meter Reconfiguration (Y)", StringComparison.OrdinalIgnoreCase))
                        {
                            OutputLiteral.Text += "Admin Tariff Change (TN) & Meter Reconfiguration (TM) and EWR may be required:" +

                                "<br />" +
                                "Check if an additional tariff is being added or a tariff is being removed - EWR<br />" +
                                "Check if wants to combine both to T31 - Admin Tariff Change(TN) &Meter Reconfiguration(Y)"+
                                "<ul>" +
                                "<li>There is no fee for site visit reconfig or remote reconfig to the customer.</li>" +
                                "<li>Advise the customer that the meter provider will do a desktop analysis and attempt to change the tariff either remotely or on-site and, in some cases, we may contact you if an EWR is required</li>" +
                                "</ul>"+
                                "Panviva <a href='#' onclick='openFeeLink(); return false;'>Tas Metering fees</a>";
                        }

                        else if (process.Equals("Meter Reconfiguration (TM) and EWR may be required", StringComparison.OrdinalIgnoreCase))
                        {
                            OutputLiteral.Text += "Admin Tariff Change (TN) & Meter Reconfiguration (TM) and EWR may be required:" +

                                "<br />" +

                                "<ul>" +
                                "<li>There is no fee for site visit reconfig or remote reconfig to the customer.</li>" +
                                "<li>Advise the customer that the meter provider will do a desktop analysis and attempt to change the tariff either remotely or on-site and, in some cases, we may contact you if an EWR is required</li>" +
                                "</ul>" +
                                "Panviva <a href='#' onclick='openFeeLink(); return false;'>Tas Metering fees</a>";

                        }

                        else if (process.Equals("Meter Reconfiguration (TM)", StringComparison.OrdinalIgnoreCase))
                        {
                            OutputLiteral.Text += "Meter Reconfiguration (TM)" +

                                "<br />" +

                                "<ul>" +
                                "<li>There is no fee for site visit reconfig or remote reconfig to the customer.</li>" +
                                "</ul>" +
                                "Panviva <a href='#' onclick='openFeeLink(); return false;'>Tas Metering fees</a>";
                        }
                    }
                }
            }
        }

       
    }
}
