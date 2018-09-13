# ShortUrlParser
将短网址解析到其原始地址的 C# 类库，附带识别一个短网址是不是 gbf 的功能。
## 功能
 - 将编码后的（url-encoded）网址解码
 - 解析一个短链接的原始地址
 - 循环解析嵌套的短链接，并且允许指定循环次数
 - 识别一个短网址是不是 gbf
## 用法
``` C#
using ShortUrlParse;
ShortUrlParser parser = new ShortUrlParser();

// 解析一个短网址的实际地址
string longUrl = parser.Parse("http://t.cn/EvAfusa");

// 解析一个嵌套短链接，并且最多跳转10次
string longUrl2 = parser.Parse("http://t.cn/EvAfusa", 10);

// 判断一个短链接是否指向GBF网站
bool isGbf = parser.IsGranBlueFantasy("http://%73%69%6E%61%2E%6C%74/fGqe");
```
还可以与二维码解析库（如 Zxing 等）配合使用，可以参考 ShortUrlParserSample 中的有关示例。
## 注意事项
- 解析短链功能需要联网使用，并且如果解析的短链需要特殊网络环境（如 Twitter 提供的短链接）则您需要首先配置好系统的网络设置（如代理服务器等）
- 基于 .NET Standard 2.0 开发，直接编译需要安装 .NET Core 开发环境；但也可以直接拷贝源码到新的 .NET Framework 工程中进行编译