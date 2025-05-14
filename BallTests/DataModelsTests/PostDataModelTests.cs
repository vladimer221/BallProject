using BallContratcs.DataModels;
using BallContratcs.Enums;
using BallContratcs.Exceptions;

namespace BallTests.DataModelsTests;

[TestFixture]
internal class PostDataModelTests
{
	[Test]
	public void IdIsNullOrEmptyTest()
	{
		var post = CreateDataModel(null, "name", PostType.Manager, 10, true, DateTime.UtcNow);
		Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
		post = CreateDataModel(string.Empty, "name", PostType.Manager, 10, true, DateTime.UtcNow);
		Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void IdIsNotGuidTest()
	{
		var post = CreateDataModel("id", "name", PostType.Manager, 10, true, DateTime.UtcNow);
		Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void PostNameIsEmptyTest()
	{
		var manufacturer = CreateDataModel(Guid.NewGuid().ToString(), null, PostType.Manager, 10, true, DateTime.UtcNow);
		Assert.That(() => manufacturer.Validate(), Throws.TypeOf<ValidationException>());
		manufacturer = CreateDataModel(Guid.NewGuid().ToString(), string.Empty, PostType.Manager, 10, true, DateTime.UtcNow);
		Assert.That(() => manufacturer.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void PostTypeIsNoneTest()
	{
		var post = CreateDataModel(Guid.NewGuid().ToString(), "name", PostType.None, 10, true, DateTime.UtcNow);
		Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void SalaryIsLessOrZeroTest()
	{
		var post = CreateDataModel(Guid.NewGuid().ToString(), "name", PostType.Manager, 0, true, DateTime.UtcNow);
		Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
		post = CreateDataModel(Guid.NewGuid().ToString(), "name", PostType.Manager, -10, true, DateTime.UtcNow);
		Assert.That(() => post.Validate(), Throws.TypeOf<ValidationException>());
	}

	[Test]
	public void AllFieldsIsCorrectTest()
	{
		var postId = Guid.NewGuid().ToString();
		var postName = "name";
		var postType = PostType.Manager;
		var salary = 10;
		var isActual = false;
		var changeDate = DateTime.UtcNow.AddDays(-1);
		var post = CreateDataModel(postId, postName, postType, salary, isActual, changeDate);
		Assert.That(() => post.Validate(), Throws.Nothing);
		Assert.Multiple(() =>
		{
			Assert.That(post.Id, Is.EqualTo(postId));
			Assert.That(post.PostName, Is.EqualTo(postName));
			Assert.That(post.PostType, Is.EqualTo(postType));
			Assert.That(post.Salary, Is.EqualTo(salary));
			Assert.That(post.IsActual, Is.EqualTo(isActual));
			Assert.That(post.ChangeDate, Is.EqualTo(changeDate));
		});
	}

	private static PostDataModel CreateDataModel(string? id, string? postName, PostType postType, double salary, bool isActual, DateTime changeDate) =>
		new(id, postName, postType, salary, isActual, changeDate);
}