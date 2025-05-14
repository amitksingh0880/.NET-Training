<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxToolkit.aspx.cs" Inherits="CountryCodeApplication.AjaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Accordion Example</title>
    <style type="text/css">
        .accordionHeader {
            background-color: #e0e0e0;
            padding: 5px;
            font-weight: bold;
        }

        .accordionHeaderSelected {
            background-color: #c0c0c0;
        }

        .accordionContent {
            padding: 10px;
            background-color: #f9f9f9;
        }

        .cssClass1 { color: red; }
.cssClass2 { color: orange; }
.cssClass3 { color: goldenrod; }
.cssClass4 { color: green; }
.cssClass5 { color: darkgreen; }
.draggablePanel {
    position: absolute;
    background-color: #fff;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
}

.dragHandle {
    font-weight: bold;
}


    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <ajaxToolkit:Accordion
            ID="MyAccordion"
            runat="server"
            SelectedIndex="0"
            HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent"
            AutoSize="None"
            FadeTransitions="true"
            TransitionDuration="250"
            FramesPerSecond="40"
            RequireOpenedPane="false"
            SuppressHeaderPostbacks="true">
            
            <Panes>
                <ajaxToolkit:AccordionPane runat="server">
                    <Header>Section 1</Header>
                    <Content>
                        <asp:Label ID="Label1" runat="server" Text="Content for section 1"></asp:Label>
                    </Content>
                </ajaxToolkit:AccordionPane>

                <ajaxToolkit:AccordionPane runat="server">
                    <Header>Section 2</Header>
                    <Content>
                        <asp:Label ID="Label2" runat="server" Text="Content for section 2"></asp:Label>
                    </Content>
                </ajaxToolkit:AccordionPane>
            </Panes>
        </ajaxToolkit:Accordion>



        <ajaxToolkit:ComboBox 
    ID="ComboBox1" 
    runat="server" 
    DropDownStyle="DropDown" 
    AutoCompleteMode="None"
    CaseSensitive="false"
    RenderMode="Inline"
    ItemInsertLocation="Append"
    ListItemHoverCssClass="ComboBoxListItemHover">

    <asp:ListItem Text="Option 1" Value="1"></asp:ListItem>
    <asp:ListItem Text="Option 2" Value="2"></asp:ListItem>
    <asp:ListItem Text="Option 3" Value="3"></asp:ListItem>

</ajaxToolkit:ComboBox>
         

         <asp:TextBox ID="TextBox1" runat="server" TextMode="Password" />
         <ajaxToolkit:PasswordStrength 
    ID="PS" 
    runat="server"
    TargetControlID="TextBox1"
    DisplayPosition="RightSide"
    StrengthIndicatorType="Text"
    PreferredPasswordLength="10"
    PrefixText="Strength:"
    TextCssClass="TextIndicator_TextBox1"
    MinimumNumericCharacters="0"
    MinimumSymbolCharacters="0"
    RequiresUpperAndLowerCaseCharacters="false"
    TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
    TextStrengthDescriptionStyles="cssClass1;cssClass2;cssClass3;cssClass4;cssClass5"
    CalculationWeightings="50;15;15;20" />


        <asp:Panel ID="Panel3" runat="server" CssClass="draggablePanel" style="width:300px; border:1px solid #ccc; padding:10px;">
    
    <asp:Panel ID="Panel4" runat="server" CssClass="dragHandle" style="cursor:move; background-color:#e0e0e0; padding:5px;">
        Drag me!
    </asp:Panel>


    <div>
        This is some content inside the draggable panel.jsdjgbvshhgnsdgkhkendkghivhnkghonksjdhgiknvih 
        lsnvgknjknknrkgjfvnkliowejrgnihncvnwjkfc knjkghtkjvklsnkgolenlvgfohjkcvnskhkjkfdjvkwhjkvg
        skdjandvgfnskdnvgjknfkngbv
    </div>
</asp:Panel>

<ajaxToolkit:DragPanelExtender 
    ID="DPE1" 
    runat="server"
    TargetControlID="Panel3"
    DragHandleID="Panel4" />


        <asp:LinkButton ID="LinkButton1" runat="server" >
    Click Me
</asp:LinkButton>

<!-- ConfirmButtonExtender attached to the LinkButton -->
<ajaxToolkit:ConfirmButtonExtender 
    ID="cbe" 
    runat="server"
    TargetControlID="LinkButton1"
    ConfirmText="Are you sure you want to click this?"
    OnClientCancel="CancelClick" />
    </form>
</body>
</html>
