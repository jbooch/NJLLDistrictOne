<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="PastChampions.aspx.cs" Inherits="PastChampions" Title="District One Past Champions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="row" style="background-color:lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Past Champions</span><br />
            </div>
        </div>
    </div>
    <div class="container">
        <br />
        <div class="row">
            <div class="col-sm-6 text-center">
                <asp:Button CssClass="btn btn-outline-dark" ID="btnMaintenance" runat="server" OnClick="btnMaintenance_Click" Text="Update Champions" />
            </div>
            <div class="col-sm-6 text-center">
                <asp:Button CssClass="btn btn-outline-success" ID="btnPastChamps" runat="server" Text="Past Champs View" OnClick="btnPastChamps_Click" CausesValidation="false" />
            </div>
        </div>
        <br />
    </div>
             <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate> 
                    <asp:MultiView ID="mvPastChamps" runat="server">
                        <asp:View ID="vwPastChamps" runat="server">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="table-responsive">
                                            <asp:Repeater ID="rpChampions" runat="server" OnItemDataBound="rpChampions_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table class="table table-bordered table-sm">
                                                        <thead>
                                                            <tr>
                                                                <th class="text-center" rowspan="2">&nbsp;</th>
                                                                <th class="text-center" colspan="6">
                                                                    <h4 class="h4">Baseball</h4>
                                                                </th>
                                                                <th class="text-center" colspan="5">
                                                                    <h4 class="h4">Softball</h4>
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <th class="align-bottom">9/10</th>
                                                                <th class="align-bottom">10/11</th>
                                                                <th class="align-bottom">Little League</th>
                                                                <th class="align-bottom">Intermediate (50/70)</th>
                                                                <th class="align-bottom">Junior</th>
                                                                <th class="align-bottom">Senior</th>
                                                                <th class="align-bottom">9/10</th>
                                                                <th class="align-bottom">10/11</th>
                                                                <th class="align-bottom">Little League</th>
                                                                <th class="align-bottom">Junior</th>
                                                                <th class="align-bottom">Senior</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <h5 class="h5">
                                                                <asp:Label ID="lblYear" runat="server" Text='<%#Bind("year") %>'></asp:Label></h5>
                                                        </td>
                                                        <td id="td0" runat="server">
                                                            <asp:Label CssClass="text-center font-weight-bold" ID="lblBaseballMinors" runat="server" Text='<%#Bind("bbMinors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lblSectionsbbMinors" runat="server" Text="Sections" Visible='<%#Bind("bbMinors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label48" runat="server" Text="States" Visible='<%#Bind("bbMinors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label49" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("bbMinors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label50" runat="server" Text="National" Visible='<%#Bind("bbMinors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label1" runat="server" Text='<%#Bind("bbMinors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId0" runat="server" Text='<%#Bind("bbMinors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td1" runat="server">
                                                            <asp:Label ID="lblBaseballMinors2" runat="server" Text='<%#Bind("bbMinors2.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label51" runat="server" Text="Sections" Visible='<%#Bind("bbMinors2.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label52" runat="server" Text="States" Visible='<%#Bind("bbMinors2.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label53" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("bbMinors2.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label54" runat="server" Text="National" Visible='<%#Bind("bbMinors2.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lblbbMinors2OtherText" runat="server" Text='<%#Bind("bbMinors2.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId1" runat="server" Text='<%#Bind("bbMinors2.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td2" runat="server">
                                                            <asp:Label ID="lblBaseballMajors" runat="server" Text='<%#Bind("bbMajors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label55" runat="server" Text="Section Champs" Visible='<%#Bind("bbMajors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label56" runat="server" Text="State Champs" Visible='<%#Bind("bbMajors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label57" runat="server" Text="Mid-Atlantic Champs" Visible='<%#Bind("bbMajors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label58" runat="server" Text="National Champs" Visible='<%#Bind("bbMajors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lblbbMajorsOtherText" runat="server" Text='<%#Bind("bbMajors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId2" runat="server" Text='<%#Bind("bbMajors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td10" runat="server">
                                                            <asp:Label ID="Label86" runat="server" Text='<%#Bind("bbIntermediate.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label87" runat="server" Text="Sections" Visible='<%#Bind("bbIntermediate.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label88" runat="server" Text="States" Visible='<%#Bind("bbIntermediate.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label89" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("bbIntermediate.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label90" runat="server" Text="National" Visible='<%#Bind("bbIntermediate.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label91" runat="server" Text='<%#Bind("bbIntermediate.otherText")%>'></asp:Label>
                                                            <asp:Label ID="Label92" runat="server" Text='<%#Bind("bbIntermediate.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td3" runat="server">
                                                            <asp:Label ID="lblBaseballJuniors" runat="server" Text='<%#Bind("bbJuniors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label59" runat="server" Text="Sections" Visible='<%#Bind("bbJuniors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label60" runat="server" Text="States" Visible='<%#Bind("bbJuniors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label61" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("bbJuniors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label62" runat="server" Text="National" Visible='<%#Bind("bbJuniors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label4" runat="server" Text='<%#Bind("bbJuniors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId3" runat="server" Text='<%#Bind("bbJuniors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td4" runat="server">
                                                            <asp:Label ID="lblBaseballSeniors" runat="server" Text='<%#Bind("bbSeniors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label63" runat="server" Text="Sections" Visible='<%#Bind("bbSeniors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label64" runat="server" Text="States" Visible='<%#Bind("bbSeniors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label65" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("bbSeniors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label66" runat="server" Text="National" Visible='<%#Bind("bbSeniors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label5" runat="server" Text='<%#Bind("bbSeniors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId4" runat="server" Text='<%#Bind("bbSeniors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td5" runat="server">
                                                            <asp:Label ID="lblSoftballMinors" runat="server" Text='<%#Bind("sbMinors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label2" runat="server" Text="Sections" Visible='<%#Bind("sbMinors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label67" runat="server" Text="States" Visible='<%#Bind("sbMinors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label68" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("sbMinors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label69" runat="server" Text="National" Visible='<%#Bind("sbMinors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label6" runat="server" Text='<%#Bind("sbMinors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId5" runat="server" Text='<%#Bind("sbMinors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td6" runat="server">
                                                            <asp:Label ID="lblSoftballMinors2" runat="server" Text='<%#Bind("sbMinors2.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label70" runat="server" Text="Sections" Visible='<%#Bind("sbMinors2.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label71" runat="server" Text="States" Visible='<%#Bind("sbMinors2.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label72" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("sbMinors2.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label73" runat="server" Text="National" Visible='<%#Bind("sbMinors2.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label7" runat="server" Text='<%#Bind("sbMinors2.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId6" runat="server" Text='<%#Bind("sbMinors2.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td7" runat="server">
                                                            <asp:Label ID="lblSoftballMajors" runat="server" Text='<%#Bind("sbMajors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label74" runat="server" Text="Sections" Visible='<%#Bind("sbMajors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label75" runat="server" Text="States" Visible='<%#Bind("sbMajors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label76" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("sbMajors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label77" runat="server" Text="National" Visible='<%#Bind("sbMajors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label8" runat="server" Text='<%#Bind("sbMajors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId7" runat="server" Text='<%#Bind("sbMajors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td8" runat="server">
                                                            <asp:Label ID="lblSoftballJuniors" runat="server" Text='<%#Bind("sbJuniors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label78" runat="server" Text="Sections" Visible='<%#Bind("sbJuniors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label79" runat="server" Text="States" Visible='<%#Bind("sbJuniors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label80" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("sbJuniors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label81" runat="server" Text="National" Visible='<%#Bind("sbJuniors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label9" runat="server" Text='<%#Bind("sbJuniors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId8" runat="server" Text='<%#Bind("sbJuniors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="td9" runat="server">
                                                            <asp:Label ID="lblSoftballSeniors" runat="server" Text='<%#Bind("sbSeniors.team")%>'></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label82" runat="server" Text="Sections" Visible='<%#Bind("sbSeniors.sections")%>' ForeColor="red"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label83" runat="server" Text="States" Visible='<%#Bind("sbSeniors.states")%>' ForeColor="blue"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label84" runat="server" Text="Mid-Atlantic" Visible='<%#Bind("sbSeniors.regions")%>' ForeColor="Olive"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label85" runat="server" Text="National" Visible='<%#Bind("sbSeniors.nationals")%>' ForeColor="BlueViolet"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="Label10" runat="server" Text='<%#Bind("sbSeniors.otherText")%>'></asp:Label>
                                                            <asp:Label ID="champId9" runat="server" Text='<%#Bind("sbSeniors.id")%>' Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
        </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="vwMaintenance" runat="server">
                            <div class="container">
                                <div class="row">
                                    <div class="class=col-sm-12 text-sm-center">
                                        <asp:Label ID="Label1" runat="server" Text="Season"></asp:Label>&nbsp;<asp:DropDownList ID="dlChampSeason" runat="server" Width="130px" AutoPostBack="True" OnSelectedIndexChanged="dlChampSeason_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <div class="row border border-dark p-2">
                                    <div class="col-sm-12">
                                        <br />
                                        <div class="row">
                                            <div class="col-sm-5 l-2 text-center">
                                                <div class="input-group">
                                                    <span class="input-group-text">Year:</span>
                                                    <asp:TextBox ID="txtYear" CssClass="form-control" placeholder="CCYY" runat="server" Width="51px"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditValidator ID="txtYearValidator" runat="server" ControlExtender="meYearExtender" ControlToValidate="txtYear" EmptyValueMessage="Year is Required" ErrorMessage="Invalid Year" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Year" EmptyValueBlurredText="*" IsValidEmpty="false" MaximumValueBlurredMessage="Year is not Valid" MaximumValueMessage="Year is not Valid" MinimumValueBlurredText="Year is not Valid" MinimumValueMessage="Year is not Valid" SetFocusOnError="True"></ajaxToolkit:MaskedEditValidator>
                                                    <ajaxToolkit:MaskedEditExtender ID="meYearExtender" runat="server" AcceptNegative="None" Mask="9999" TargetControlID="txtYear" MaskType="number"></ajaxToolkit:MaskedEditExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row border border-primary">
                                            <!-- Baseball 9/10 -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text">Baseball 9/10</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtbbMinorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="ckbbMinorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtbbMinorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Baseball 10/11 -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Baseball 10/11</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtbbMinors2Team" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="ckbbMinors2Winnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtbbMinors2OtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Baseball 11/12 -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Baseball Little League</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtbbMajorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="ckbbMajorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtbbMajorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Baseball Intermediate -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Baseball Intermediate</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtbbIntermediateTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="ckbbIntermediateWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtbbIntermediateOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Baseball Juniors -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Baseball Juniors</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtbbJuniorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="ckbbJuniorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtbbJuniorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Baseball Seniors -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Baseball Seniors</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtbbSeniorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="ckbbSeniorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtbbSeniorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Softball Minors -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Softball 9/10</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtsbMinorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="cksbMinorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtsbMinorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Softball 10/11 -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Softball 10/11</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtsbMinors2Team" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="cksbMinors2Winnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtsbMinors2OtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Softball 11/12 -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Softball Little League</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtsbMajorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="cksbMajorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtsbMajorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Softball Juniors -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Softball Juniors</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtsbJuniorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="cksbJuniorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtsbJuniorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row border border-primary">
                                            <!-- Softball Seniors -->
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class=" col-sm-5">
                                                        <div class="input-group">
                                                            <span class="input-group-text text-dark">Softball Seniors</span>
                                                            <asp:TextBox CssClass="form-control" ID="txtsbSeniorsTeam" placeholder="Team Name" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-7 input-group">
                                                        <span class="input-group-text">Additional Championships</span>
                                                        <asp:CheckBoxList ID="cksbSeniorsWinnings" CssClass="form-control list-group list-group-horizontal" runat="server" RepeatDirection="Horizontal" CellPadding="10">
                                                            <asp:ListItem>Sections</asp:ListItem>
                                                            <asp:ListItem Value="New Jersey Champs">States</asp:ListItem>
                                                            <asp:ListItem Value="Mid Atlantic Champs">Regions</asp:ListItem>
                                                            <asp:ListItem>National</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6 input-group">
                                                        <span class="input-group-text">Team Comments</span>
                                                        <asp:TextBox CssClass="form-control" ID="txtsbSeniorsOtherText" placeholder="Team Comments" TextMode="MultiLine" Rows="1" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6 text-center">
                                        <asp:Button CssClass="btn btn-outline-dark" ID="btnSave" runat="server" Text="Save Season" OnClick="btnSave_Click" />
                                    </div>
                                    <div class="col-sm-6 text-center">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancel" runat="server" Text="Reset Season" CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
               </ContentTemplate>
            </asp:UpdatePanel> 
</asp:Content>

