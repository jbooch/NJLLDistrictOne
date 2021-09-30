<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="Schedule.aspx.cs" Inherits="DistrictOne.Schedule" Title="Schedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="container">
            <div class="row" style="background-color: lavender">
                <div class="col-sm-12 text-center">
                    <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District Game Schedule</span><br />
                </div>
            </div>
        </div>
        <br />
        <div class="container">
            <div class="row border border-primary text-center">
                <div class="col-sm-12 p-1">
                    <div class="row">
                        <div class="col-sm-12">
                            <h4 class="h4">Note: To filter the schedule, use the filter fields below (list boxes are multi-select)</h4>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-center">
                            <span class="col-form-label-lg" >Level of Play</span>
                        </div>
                        <div class="col-sm-6 text-center">
                            <span class="col-form-label-lg">League</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 d-flex justify-content-center">
                            <asp:ListBox CssClass="list-group" ID="lstDivision" runat="server" SelectionMode="Multiple">
                                <asp:ListItem>Minors 9/10</asp:ListItem>
                                <asp:ListItem>Minors 10/11</asp:ListItem>
                                <asp:ListItem>Little League (11/12)</asp:ListItem>
                                <asp:ListItem>Juniors 13/14</asp:ListItem>
                                <asp:ListItem>Seniors 15/16</asp:ListItem>
                            </asp:ListBox>
                        </div>
                        <div class="col-sm-6 d-flex justify-content-center">
                            <asp:ListBox CssClass="list-group" ID="lstLeague" runat="server" SelectionMode="Multiple">
                                <asp:ListItem>Par-Troy West Little League</asp:ListItem>
                                <asp:ListItem>Par-Troy Little League East</asp:ListItem>
                                <asp:ListItem>Tri-Town</asp:ListItem>
                                <asp:ListItem>Rockaway</asp:ListItem>
                            </asp:ListBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 text-center p-1">
                            <asp:Button CssClass="btn btn-outline-dark" ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
                            <asp:Button CssClass="btn btn-outline-dark" ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row border border-dark p-2">
                <!-- Nav Row -->
                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <span class="h4">Schedule Navigation</span>
                        </div>
                    </div>
                    <br />
                    <div class="row justify-content-between">
                        <asp:LinkButton ID="lnkFirst" CssClass="col-sm-1 btn btn-outline-dark p-2" runat="server" OnClick="btnFirst_Click">[First]</asp:LinkButton>
                        <asp:LinkButton ID="lnkPrevious" CssClass="col-sm-1 btn btn-outline-dark p-2" runat="server" OnClick="btnPrev_Click">[Previous]</asp:LinkButton>
                        <asp:DropDownList ID="dlPage" CssClass="col-sm-2 dropdown p-2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dlPage_SelectedIndexChanged"></asp:DropDownList>
                        <asp:LinkButton ID="lnkNext" CssClass="col-sm-1 btn btn-outline-dark p-2" runat="server" OnClick="btnNext_Click">[Next]</asp:LinkButton>
                        <asp:LinkButton ID="lnkLast" CssClass="col-sm-1 btn btn-outline-dark p-2" runat="server" OnClick="btnLast_Click">[Last]</asp:LinkButton>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="table-responsive">
                                <asp:GridView HeaderStyle-CssClass="table table-dark text-white" CssClass="table table-hover p-1" ID="grdSchedule" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="grdSchedule_SelectedIndexChanged" AllowPaging="True" Font-Size="Medium" OnSorted="grdSchedule_Sorted" OnSorting="grdSchedule_Sorting" AllowSorting="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Report Results" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-outline-dark" ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                                    Text="Results" Visible='<% # Bind("resultAllowed") %>'></asp:LinkButton>
                                                <asp:Label ID="RecordId" runat="server" Text='<% #Bind("id") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="gameDate" HeaderText="Date/Time" SortExpression="gameDate" DataFormatString="{0:&quot;ddd MM/dd/yyyy hh:mm tt&quot;}">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Division" HeaderText="Level of Play" SortExpression="division">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="poolPlay" HeaderText="Pool" SortExpression="PoolPlay">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="visitor" HeaderText="Visitor" SortExpression="visitor">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VisitorScore" HeaderText="Score">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="home" HeaderText="Home" SortExpression="home">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HomeScore" HeaderText="Score">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="location" HeaderText="Location" SortExpression="location">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="updateComment" HeaderText="Last Update Comment">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="#FFC080" />
                                    <PagerSettings Visible="False" />
                                </asp:GridView>

                            </div>
                         </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:Label CssClass="text-lg-center" ID="lblPage" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div> <!-- Nav Close -->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>    
</asp:Content>
