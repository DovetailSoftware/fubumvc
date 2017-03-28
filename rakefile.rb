COMPILE_TARGET = ENV['config'].nil? ? "debug" : ENV['config']
BUILD_VERSION = '100.0.0'
tc_build_number = ENV["BUILD_NUMBER"]
build_revision = tc_build_number || Time.new.strftime('5%H%M')
build_number = "#{BUILD_VERSION}.#{build_revision}"
BUILD_NUMBER = build_number 
msbuild = '"C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"'

desc 'Compile the code'
task :compile => [:clean, :version] do
  sh "#{msbuild} src/FubuMVC.sln /property:Configuration=#{COMPILE_TARGET} /v:m /t:rebuild /nr:False /maxcpucount:8"
end

desc "Update the version information for the build"
task :version do
  asm_version = build_number
  
  begin
    commit = `git log -1 --pretty=format:%H`
  rescue
    commit = "git unavailable"
  end
  puts "##teamcity[buildNumber '#{build_number}']" unless tc_build_number.nil?
  puts "Version: #{build_number}" if tc_build_number.nil?
  
  options = {
    :description => 'FubuMVC',
    :product_name => 'FubuMVC',
    :copyright => 'Copyright 2008-2010 Chad Myers, Jeremy D. Miller, Joshua Flanagan, et al. All rights reserved.',
    :trademark => commit,
    :version => asm_version,
    :file_version => build_number,
    :informational_version => asm_version
  }
  
  puts "Writing src/CommonAssemblyInfo.cs..."
  File.open('src/CommonAssemblyInfo.cs', 'w') do |file|
    file.write "using System.Reflection;\n"
    file.write "using System.Runtime.InteropServices;\n"
    file.write "[assembly: AssemblyDescription(\"#{options[:description]}\")]\n"
    file.write "[assembly: AssemblyProduct(\"#{options[:product_name]}\")]\n"
    file.write "[assembly: AssemblyCopyright(\"#{options[:copyright]}\")]\n"
    file.write "[assembly: AssemblyTrademark(\"#{options[:trademark]}\")]\n"
    file.write "[assembly: AssemblyVersion(\"#{options[:version]}\")]\n"
    file.write "[assembly: AssemblyFileVersion(\"#{options[:file_version]}\")]\n"
    file.write "[assembly: AssemblyInformationalVersion(\"#{options[:informational_version]}\")]\n"
  end
end

desc "Prepares the working directory for a new build"
task :clean do
  FileUtils.rm_rf 'artifacts'
  Dir.mkdir 'artifacts'
end

desc 'Run the unit tests'
task :test => [:compile, :fast_test] do
end

desc 'Run the unit tests without compile'
task :fast_test do
  # Note: src/FubuMVC.AspNetTesting/bin/#{COMPILE_TARGET}/FubuMVC.AspNetTesting.dll is currently build for IIS6 and blows up with IIS7 and later unless you have IIS WMI installed.
  #TODO: Figure out a better way to do the ASP.NET testing (IISExpress?)
  sh "src/packages/NUnit.ConsoleRunner.3.6.1/tools/nunit3-console.exe src/FubuMVC.Tests/bin/#{COMPILE_TARGET}/FubuMVC.Tests.dll src/FubuMVC.SelfHost.Testing/bin/#{COMPILE_TARGET}/FubuMVC.SelfHost.Testing.dll src/FubuMVC.OwinHost.Testing/bin/#{COMPILE_TARGET}/FubuMVC.OwinHost.Testing.dll src/FubuMVC.StructureMap.Testing/bin/#{COMPILE_TARGET}/FubuMVC.StructureMap.Testing.dll"
end

desc 'Run the integration tests'
task :integration_test => [:compile, :fast_integration_test]

desc "Runs the integration tests"
task :fast_integration_test => :compile do
  sh "src/packages/NUnit.ConsoleRunner.3.6.1/tools/nunit3-console.exe src/FubuMVC.IntegrationTesting/bin/#{COMPILE_TARGET}/FubuMVC.IntegrationTesting.dll"
end