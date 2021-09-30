<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="DistrictLeagues.aspx.cs" Inherits="DistrictLeagues" Title="DistrictOne Leagues" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Leagues</span>
            </div>
        </div>
    </div>
    <div class="container">
        <br />
        <div class="row">
            <div class="col-sm-12">
                <asp:Button CssClass="btn btn-dark" ID="btnAddLeague" runat="server" Text="Add League" ToolTip="Add a League to the District" OnClick="btnAddLeague_Click" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-sm-12">
                <asp:MultiView ID="mViewLeagues" runat="server">
                    <asp:View ID="vwLeagues" runat="server">
                        <div class="table-responsive">
                            <asp:GridView HeaderStyle-CssClass="table-dark text-center" CssClass="table table-active table-hover table-bordered" ID="grdLeagues" runat="server" AutoGenerateColumns="False" OnRowEditing="grdLeagues_RowEditing" OnRowDeleting="grdLeagues_RowDeleting">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn btn-outline-dark btn-block" ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                Text="Edit" />
                                            <asp:Button CssClass="btn btn-outline-danger btn-block" ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="League Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeagueId" runat="server" Text='<% # Bind("id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblName" runat="server" Text='<% #Bind("websiteURL") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True"
                                            HorizontalAlign="Center" VerticalAlign="Bottom" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="town" HeaderText="Town">
                                        <HeaderStyle Font-Bold="True"
                                            HorizontalAlign="Center" VerticalAlign="Bottom" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="charterYear" HeaderText="Charter Year">
                                        <HeaderStyle Font-Bold="True"
                                            HorizontalAlign="Center" VerticalAlign="Bottom" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="President">
                                        <HeaderStyle Font-Bold="True"
                                            HorizontalAlign="Center" VerticalAlign="Bottom" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblPresident" runat="server" Text='<% #Bind("presidentURL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Directions">
                                        <HeaderStyle Font-Bold="True"
                                            HorizontalAlign="Center" VerticalAlign="Bottom" />
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlDirections" runat="server" NavigateUrl='<%#Bind("directions") %>' Target="_blank" Text="Directions"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:View>
                    <asp:View ID="vwLeagueMaintenance" runat="server">
                        <div class="row border border-dark p-1">
                            <div class="col-sm-12">
                                <div class="form-group row">
                                    <asp:Label runat="server" CssClass="col-sm-1 col-form-label-lg" ID="Label1" Text="Name: "></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtLeagueName"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLeagueName"
                                            ErrorMessage="Name is Required"></asp:RequiredFieldValidator>
                                    </div>
                                    <asp:Label ID="Label2" CssClass="col-sm-1 col-form-label-lg" runat="server" Text="Town: "></asp:Label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtTown" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label ID="Label11" CssClass="col-sm-2 col-form-label-lg" runat="server" Text="League Id: "></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLeagueId" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label-lg" ID="Label3" Text="Web Site"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox placeholder="Valid Web URL" CssClass="form-control" runat="server" ID="txtWebSite"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rgWebSiteValidator" runat="server" ControlToValidate="txtWebSite"
                                            ErrorMessage="Web Site is Invalid" ValidationExpression="([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator>
                                    </div>
                                    <asp:Label ID="Label4" CssClass="col-sm-2 col-form-label-lg" runat="server" Text="Charter Year: "></asp:Label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtCharterYear" CssClass="form-control" placeholder="XXXX" runat="server"></asp:TextBox>
                                        <asp:RangeValidator ID="rgvYear" runat="server" ControlToValidate="txtCharterYear" ErrorMessage="Invalid Charter Year" MinimumValue="1950" Type="Integer"></asp:RangeValidator>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label ID="Label12" CssClass="col-sm-2 col-form-label-lg" runat="server" Text="Directions: "></asp:Label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtDirections" CssClass="form-control" runat="server" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <h3 class="h3 text-center"><asp:Label ID="Label5" runat="server" Text="League President"></asp:Label></h3>
                            </div>
                        </div>
                        <div class="row border border-dark p-1">
                            <div class="col-sm-12">
                                <div class="form-group row">
                                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label-lg" ID="Label6" Text="First Name: "></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtFirstName"></asp:TextBox>
                                    </div>
                                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label-lg" ID="Label7" Text="Last Name: "></asp:Label>

                                    <div class="col-sm-4">
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtLastName" Width="229px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label-lg" ID="Label8" Text="Home Phone: "></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox runat="server" placeholder="(xxx)xxx-xxxx" CssClass="form-control" ID="txtHomePhone"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rgHomePhone" runat="server" ControlToValidate="txtHomePhone"
                                            ErrorMessage="Home Phone is Invalid" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                                    </div>
                                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label-lg" ID="Label9" Text="Cell Phone: "></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox runat="server"  placeholder="(xxx)xxx-xxxx" CssClass="form-control" ID="txtCellPhone"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rgCellPhone" runat="server" ControlToValidate="txtCellPhone"
                                            ErrorMessage="Cell Phone is Invalid" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label runat="server" CssClass="col-sm-2 col-form-label-lg" ID="Label10" Text="Email Address: "></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="Valid Email Address" ID="txtEmailAddress"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="rgEmail" runat="server" ControlToValidate="txtEmailAddress"
                                            ErrorMessage="Invalid Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:Button runat="server" CssClass="btn btn-outline-dark" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btnRemove" CssClass="btn btn-outline-danger" runat="server" OnClick="btnRemove_Click" Text="Remove" />
                                <asp:Button runat="server" CssClass="btn btn-outline-secondary" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>

