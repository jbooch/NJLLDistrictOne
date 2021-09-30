<%@ Page Title="New Jersey District One" Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="SponsorMaintenance.aspx.cs" Inherits="SponsorMaintenance"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Sponsor Maintenance</span><br />
            </div>
        </div>
    </div>
    <div class="container">
<!--        <asp:UpdatePanel ID="updatePanelOutside" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
-->
                <br />
                <div class="row border border-dark p-2">
                    <div class="col-sm-12">
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:ListBox ID="lstSponsors" runat="server" CssClass="list-group-horizontal-sm" AutoPostBack="True" OnTextChanged="lstSponsors_TextChanged"></asp:ListBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblTitle" runat="server">Company Name: </asp:Label>
                                    <asp:TextBox CssClass="form-control" Placeholder="Company Name" ID="txtCoName" runat="server" AutoPostBack="true" OnTextChanged="txtCoName_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="input-group-text" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Company Name is Required" ControlToValidate="txtCoName" Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblCoContact" runat="server">Contact Name: </asp:Label>
                                    <asp:TextBox CssClass="form-control" placeholder="Full Name" ID="txtContactName" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblAddress" runat="server">Address</asp:Label>
                                    <asp:TextBox CssClass="form-control" ID="txtStreet1" runat="server" placeholder="Street"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="offset-1 col-sm-11">
                                <div class="input-group">
                                    <asp:TextBox CssClass="form-control" ID="txtStreet2" runat="server" placeholder="Street 2"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="offset-1 col-sm-11">
                                <div class="input-group">
                                    <asp:TextBox CssClass="form-control" ID="txtCity" runat="server" placeholder="City"></asp:TextBox>
                                    <asp:DropDownList CssClass="form-control" ID="dlState" runat="server" Width="2"></asp:DropDownList>
                                    <asp:TextBox CssClass="form-control" ID="txtZipCode" runat="server" placeholder="Zip"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblEmail" runat="server">Email Address: </asp:Label>
                                    <asp:TextBox CssClass="form-control" Placeholder="Email" ID="txtEmail" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblPhone" runat="server">Phone Number: </asp:Label>
                                    <asp:TextBox CssClass="form-control" placeholder="###-###-####" ID="txtPhone" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="input-group">
                                    <asp:Label CssClass="input-group-text" ID="lblWebSite" runat="server">Web Site:</asp:Label>
                                    <asp:TextBox CssClass="form-control" ID="txtWebSite" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12">
<!--
                                <asp:UpdatePanel ID="updateFile" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
-->
                                        <div class="input-group">
                                            <asp:FileUpload ID="imgSponsorFL" runat="server" />
                                            <asp:Image runat="server" ID="imgSponsor" AlternateText="No Image" CssClass="img-fluid" Width="250px" />
                                            <asp:CheckBox ID="ckRemove" runat="server" Text="Remove" CssClass="text-center" AutoPostBack="true" OnCheckedChanged="ckRemove_CheckedChanged" />
                                        </div>
<!--
                                    </ContentTemplate>
                                </asp:UpdatePanel>
-->
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-outline-dark" Text="Save Sponsors" OnClick="btnSave_Click" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-outline-danger" OnClick="btnDelete_Click" CausesValidation="False" />
                            </div>
                        </div>
                    </div>
                </div>
<!--
            </ContentTemplate>
        </asp:UpdatePanel>
-->
        <div class="row">
            <div class="offset-2 col-sm-10">
                <asp:ValidationSummary CssClass="text-danger" ID="ValidationSummary1" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
