<%@ Page Language="C#" MasterPageFile="~/masterMain.Master" AutoEventWireup="true" CodeFile="ScheduleResults.aspx.cs" Inherits="DistrictOne.ScheduleResults" Title="Record Game Results" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Game Results</span>
            </div>
        </div>
    </div>
    <br />

    <div class="container">
        <div class="row border border-dark p-1">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-3 text-center">
                        <span class="h5 text-primary">Game: </span>
                        <asp:Label CssClass="h5" ID="lblGameDateTime" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-3 text-center">
                        <span class="h5">Division:</span>
                        <asp:Label CssClass="h5 text-primary" ID="lblDivision" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-3 text-center">
                        <span class="h5 text-primary">Category:</span>
                        <asp:Label CssClass="h5" ID="lblPoolBracket" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-3 text-center">
                        <span class="h5 text-primary">Level of Play:</span>
                        <asp:Label CssClass="h5" ID="lblLevelOfPlay" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <span class="col-sm-1 col-form-label text-primary">Visitor: </span>
                    <div class="col-sm-3">
                        <asp:Label ID="lblVisitor" runat="server"></asp:Label>
                    </div>
                    <span class="col-sm-1 col-form-label text-primary">Score</span>
                    <div class="col-sm-1">
                        <asp:TextBox ID="txtVisitorScore" data-toggle="tooltip" Title="Enter Score" runat="server" Width="75px" MaxLength="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVisitorScore"
                            ErrorMessage="Visitors Score Cannot be blank" ClientIDMode="Inherit" EnableTheming="True">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtVisitorScore"
                            ErrorMessage="Invalid Visitor Score" MaximumValue="99" MinimumValue="0" Type="Integer" EnableTheming="True" EnableViewState="True" SetFocusOnError="True" ClientIDMode="Inherit">*</asp:RangeValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <span class="col-sm-1 col-form-label text-primary">Home</span>
                    <div class="col-sm-3">
                        <asp:Label ID="lblHome" runat="server"></asp:Label>
                    </div>
                    <span class="col-sm-1 col-form-label text-primary">Score</span>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-text" data-toggle="tooltip" Title="Enter Team Score" ID="txtHomeScore" runat="server" Width="75px" MaxLength="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtHomeScore"
                            ErrorMessage="Home Score Cannot be blank" ViewStateMode="Inherit">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtHomeScore"
                            ErrorMessage="Home Score Invalid" MaximumValue="99" MinimumValue="0" Type="Integer">*</asp:RangeValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <span class="col-sm-1 col-form-label">Reported By:</span>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtReportedBy" data-toggle="tooltip" runat="server" Title="Your Full Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtReportedBy"
                            ErrorMessage="Reported By Cannot Be Blank">*</asp:RequiredFieldValidator>
                    </div>
                    <span class="col-sm-1 col-form-label">Report Contact</span>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtPhoneContact" runat="server" MaxLength="12" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPhoneContact"
                            ErrorMessage="Report Contact Phone Is Required">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <span class="col-sm-1 col-form-label">Comment:</span>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 text-center">
                        <asp:Button CssClass="btn btn-outline-dark" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
