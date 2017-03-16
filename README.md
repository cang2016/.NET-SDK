# StoryCLM .NET SDK

StoryCLM .NET SDK ��������� ����� ������������� ������� [StoryCLM](http://breffi.ru/ru/storyclm) c ����� ����������� �� .NET.
������ ���������� ������� �� ���� [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md) ������� [StoryCLM](http://breffi.ru/ru/storyclm) � ����������� �������� ������ � API.

���� ����� ���������� ��� ���������� ����������, ��������� ������ ��� ������ � ��� � �������� ������ SDK �� ���������� ��������.

## Frameworks:
* .NET Framework 4.5

## ���������

���������� SDK ����� ����� ���������:
* NuGet Package Manager.
* ��������� SDK ��������������� �� ����� �����������.

### NuGet

[NuGet Package Manager](https://www.nuget.org/) ������ ���� �������������� ���������� � �������� � Visual Studio 2015 ��� ����� ������ ������.

**��������� ����� NuGet Package Manager � �������������� �������**

��������� ��������� ������� � ������� NuGet Package Manager ��� ��������� SDK � ���� ��� ������������:
```
PM> Install-Package StoryCLM
```
**��������� ����� NuGet Package Manager � �������������� ���������� Visual Studio**

����� ���������� SDK � ������� ���������� Visual Studio NuGet, ��������� ��������� ��������:
* � ������������ ������� �������� ������ ������� ���� �� ������ � �������� "���������� NuGet ��������".
* � ������ ������� "StoryCLM" � ������� Enter.
* ����� ���������� ������, �������� �� ������ StoryCLM .NET SDK � ������� ������ "����������".

### Git

��������� � �������� ������� [�����������](https://github.com/storyclm/.NET-SDK) � ������� �� ������ "Clone or �ownload".

![rest Image 5](./images/1.png)

## ��������� � �������� ������

**����������� ����������� ����**

� ����, � ������� �� ������ ������������ SDK, ���������� �������� ��������� ���:
```cs
using StoryCLM.SDK;
using StoryCLM.SDK.Models;
```
**��������� API � ��������� ������ �������**

��� ���� ��� �� �������� ������ � API ������ ������� ����� ��� ������������ �� ������ ����������������� � �������� ���� �������. 

*��� �� ������ ��� ������������ API, �������� ����� ������� � ������ ��������� ���������� � �������������� � ����������� � ������� ����� ������������ � ������������� �� [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md#���������).*

**������������� ������ SCLM � ��������������**

SCLM - ��� ������� �����, ������� �������� ��� ������ ��� ������ � API.

������ �����, ���������� ������������������� � ������� ����� �������.
```cs
SCLM sclm = SCLM.Instance;
Tokent token = await sclm.AuthAsync(clientId, secret);
```
����� ������������� �����������. ����� ���������, ��� ����� "�����" ���� ���. 
����� ���� ����� �������� ����� �����, ������ ����� AuthAsync.

## �������

### �������

[�������](https://github.com/storyclm/documentation/blob/master/TABLES.md) - ��� ������������ ��������� ������.

*����� ��������� ���������� ���������� � ������� ["�������"](https://github.com/storyclm/documentation/blob/master/TABLES.md) ������������.*

**�������� ��� ������� �������**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiTable> tables = await sclm.GetTablesAsync(clientId);
```

��� ������ � �������� �������� ������ Profile. ���� ������ ����� �������������� ������ � ������� "Profile".
```cs

```






