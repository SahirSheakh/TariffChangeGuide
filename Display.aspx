<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Display.aspx.cs" Inherits="TariffChangeGuide.Display" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 640px;
            height: 176px;
        }
    </style>
</head>
<body style="text-align: center;">

    <form id="form1" runat="server">

    <div class="text-center">
    <h1 class="display-4">Tariff Change Guide</h1>
    
</div>
<br />

<!--Div 1 -->
  <div class="row">
    <div class="col-md-2 col-sm-0">
        <asp:Label ID="Label2" runat="server" Text="Select Tariff Type" Font-Bold="True" Font-Size="X-Large"></asp:Label>

    </div><br/>
       
    <div class="col-md-2 col-sm-0">
       
        <asp:RadioButton ID="RadioResidential" runat="server" Text="Residential" GroupName="tariffType"/>
        <asp:RadioButton ID="RadioBusiness" runat="server" Text="Business" GroupName="tariffType"/><br/>
        
        

    </div>
</div>
        <br/>

<!--Div 2 -->

 <div class="row">
    <div class="col-md-2 col-sm-0">

        <asp:Label ID="Label1" runat="server" Text="Select Meter Type" Font-Bold="True" Font-Size="X-Large"></asp:Label>

    </div><br/>
    <div class="col-md-2 col-sm-0">
       
        <asp:RadioButton ID="RadioBM" runat="server" Text="Basic Meter" GroupName="meterType"/>
        <asp:RadioButton ID="RadioTM" runat="server" Text="TasMetering Advance Meter" GroupName="meterType" AutoPostBack="True" OnCheckedChanged="RadioTM_CheckedChanged"/>
        <asp:RadioButton ID="RadioYM" runat="server" Text="Yurika Advance Meter" GroupName="meterType" />

    </div>
</div>
        
        <br/>

<!--Dive 3 -->
<div class="row">
      <div class="col-md-6 text-center">
                <p>Enter the tariffs From and To customer wants to change</p>
                <label>From:</label>
                
            </div>
            <div class="col-md-6 text-center">
                <div class="form-group">
                    <asp:TextBox ID="TBFrom" runat="server"></asp:TextBox>
                    
                </div>
            </div>
</div><br/>

<!--Div 4 -->

         <div class="row">
     <div class="col-md-6 text-center">
         <br/>
         <label>TO:</label>
     </div>
     <div class="col-md-6 text-center">
         <div class="form-group">
              <asp:TextBox ID="TBTo" runat="server"></asp:TextBox>
             
             <br/>
         </div>
     </div>
 </div>


<!--Dive 5 -->
<div class="row">
      <div class="col-md-6 text-center">
          <p>Enter Suffix if Applicable for Yurika Meter</p>
                <label>Suffix:</label>
                
            </div>
            <div class="col-md-6 text-center">
                <div class="form-group">
                    <asp:TextBox ID="TBsuffix" runat="server" MaxLength="6" CssClass="limitedTextBox"></asp:TextBox>
                </div>
            </div>
</div><br/>

<!--Div 6 -->
<div class="row">
    <div class="col-md-6 text-center">
        <br/>
       
    </div>

    <div class="col-md-6 text-center">
        <div class="form-group">
            <asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click" />
            <br/>
        </div>
    </div>
</div>

<!--Div 7 -->
<div class="row">
            <div class="col-md-6 text-center"><br/>
                <label>Process to Follow:</label><br /><br />
                <asp:Label ID="OutputLabel" runat="server" Text="**"></asp:Label>
                
            </div>
            <br/>
            <div class="col-md-6 text-center">
               <asp:Literal ID="OutputLiteral" runat="server" Mode="PassThrough"></asp:Literal>
            </div>
</div>

</form>
</body>
</html>
