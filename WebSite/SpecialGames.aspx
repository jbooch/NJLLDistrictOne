<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="SpecialGames.aspx.cs" Inherits="SpecialGames" Title="District One Special Games" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row" style="background-color:lavender">
            <div class="col-sm-12 text-sm-center">
                    <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                    </span><br />
            <span style="font-size: 18px; color: #000066; font-family: Calibri"> NJ District One Special Games</span><br />
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-sm-12">
                <p class="h4 text-justify">
                    The Following is a list of Regulation IX special games requests that have been proposed by District 1 league.
                </p>
                <p class="text-primary text-justify">
                    Participation in tournaments that are not approved is not only a violation of a league's charter, and as such not covered by Little League Insurance, but subjects the manger, coaches and players to the provisions of tournament regulations concerning participation in other programs which may make them ineligible to participate in Little League International tournaments.
                </p>
            </div>
        </div>
        <div class="row border border-dark">
            <div class="col-sm-12">
                <br />
                <div class="row">
                    <div class="col-sm-12">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:MultiView ID="mvSpecialGames" runat="server">

                                    <asp:View ID="View1" runat="server">
                                        <div class="row">
                                            <div class="col-sm-12 text-center">
                                                <asp:Button CssClass="btn btn-outline-primary" ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="table-responsive">
                                                    <asp:GridView CssClass="table table-active" ID="grdSpecialGames" runat="server" AutoGenerateColumns="False" OnRowEditing="grdSpecialGames_RowEditing">
                                                        <Columns>
                                                            <asp:CommandField ControlStyle-CssClass="btn btn-outline-dark" ButtonType="Button" InsertVisible="False" ShowDeleteButton="True"
                                                                ShowEditButton="True" />
                                                            <asp:BoundField HeaderText="Tournament" DataField="tournament">
                                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                                    HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Type" DataField="type">
                                                                <HeaderStyle Font-Bold="True"
                                                                    HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Age Range" DataField="ageRange">
                                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                                    HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Host League">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHoseName" runat="server" Text='<%# Bind("hostLeague.name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Bold="True"
                                                                    Font-Underline="True" HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Status" DataField="status">
                                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                                    HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Tournament Rules">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="lnkRules" NavigateUrl='<%# Bind("tournamentRules.link") %>' runat="server" Target="_blank" Text='<%# Bind("tournamentRules.name") %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                                    HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Registration Form">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="lnkForm" NavigateUrl='<%# Bind("registrationForm.link") %>' Target="_blank" runat="server" Text='<%# Bind("registrationForm.name") %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                                    HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Website">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink NavigateUrl='<%# Bind("website.link") %>' Target="_blank" ID="lnkWebSite" runat="server" Text='<%# Bind("website.name") %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <HeaderStyle Font-Bold="True" Font-Underline="True"
                                                                    HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle BackColor="Wheat" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:View>

                                    <asp:View ID="View2" runat="server">
                                        <br />
                                        <div class="form-group row">
                                            <asp:Label CssClass="col-sm-2 col-form-label" ID="Label1" runat="server" Text="Name:"></asp:Label>
                                            <div class="col-sm-4">
                                                <asp:TextBox CssClass="form-control" ID="txtName" runat="server"></asp:TextBox>
                                            </div>
                                            <asp:Label CssClass="col-sm-2 col-form-label" ID="Label2" runat="server" Text="Host League"></asp:Label>
                                            <div class="col-sm-3">
                                                <asp:DropDownList CssClass="dropdown" ID="dlLeague" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <asp:Label CssClass="col-sm-2 col-form-label" ID="Label3" runat="server" Text="Age Range"></asp:Label>
                                            <div class="col-sm-2">
                                                <asp:TextBox CssClass="form-control" ID="txtAgeRange" runat="server"></asp:TextBox>
                                            </div>
                                            <asp:Label CssClass="col-sm-2 col-form-label" ID="Label4" runat="server" Text="Type"></asp:Label>
                                            <div class="col-sm-4">
                                                <asp:RadioButtonList CssClass="form-check-inline" ID="rbType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="Baseball">Baseball</asp:ListItem>
                                                    <asp:ListItem Value="Softball">Softball</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <asp:Label CssClass="col-sm-1 col-form-label" ID="Label5" runat="server" Text="Status"></asp:Label>
                                            <div class="col-sm-4 ">
                                                <asp:RadioButtonList CssClass="dropdown p-2" ID="rbStatus" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="Submitted">Submitted</asp:ListItem>
                                                    <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                                    <asp:ListItem Value="Approved">Approved</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <asp:Label CssClas="col-sm-1 col-form-label" ID="Label6" runat="server" Text="Rules"></asp:Label>
                                            <div class="col-sm-5 ">
                                                <asp:TextBox CssClass="form-control" ID="txtRulesName" runat="server"></asp:TextBox>
                                                <div class="input-group">
                                                    <asp:TextBox CssClass="form-control" ID="txtRulesLink" runat="server"></asp:TextBox>
                                                    <asp:Button CssClass="btn btn-outline-info" ID="btnRulesLink" runat="server" OnClick="btnRulesLink_Click" Text="Upload" />
                                                </div>
                                            </div>
                                         </div>
                                        <div class="form-group row">
                                            <asp:Label CssClas="col-sm-2 col-form-label" ID="Label7" runat="server" Text="Registration Form:"></asp:Label>
                                            <div class="col-sm-5">
                                                <asp:TextBox CssClass="form-control" ID="txtRegFormName" runat="server"></asp:TextBox>
                                                <div class="input-group">
                                                    <asp:TextBox CssClass="form-control" ID="txtRegFormLink" runat="server"></asp:TextBox>
                                                    <asp:Button CssClass="btn btn-outline-info" ID="btnRegFormLink" runat="server" OnClick="btnRulesLink_Click" Text="Upload" />
                                                </div>
                                                
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <asp:Label CssClass="col-sm-1 col-form-label" ID="Label8" runat="server" Text="Web Site:"></asp:Label>
                                            <div class="col-sm-8">
                                                <asp:TextBox CssClass="form-control" ID="txtWebSiteName" runat="server"></asp:TextBox>
                                                <asp:TextBox CssClass="form-control" ID="txtWebSiteLink" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 text-center">
                                                <asp:Button CssClass="btn btn-outline-dark" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" />
                                            </div>
                                            <div class="col-sm-6 text-center">
                                                <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                                            </div>
                                        </div>

                                    </asp:View>

                                    <asp:View ID="View3" runat="server">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="input-group" >
                                                    <asp:Label CssClass="input-group-text" ID="lblUploadName" runat="server"></asp:Label>
                                                    <asp:FileUpload ID="FileUpload1"  runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 text-center">
                                                <asp:Button CssClass="btn btn-outline-dark" ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />
                                            </div>
                                            <div class="col-sm-6 text-center">
                                                <asp:Button CssClass="btn btn-outline-warning" ID="btnUploadeCancel" runat="server" OnClick="btnUploadeCancel_Click" Text="Cancel" />
                                            </div>
                                        </div>
                                    </asp:View>
                                </asp:MultiView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

