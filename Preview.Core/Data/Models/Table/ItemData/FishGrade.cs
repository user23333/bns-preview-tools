using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models;
public sealed class FishGrade : ModelElement
{
	public enum GradeSeq
	{
		[Name("fish-grade-1")] FishGrade1,
		[Name("fish-grade-2")] FishGrade2,
		[Name("fish-grade-3")] FishGrade3,
		[Name("fish-grade-4")] FishGrade4,
		[Name("fish-grade-5")] FishGrade5,
		[Name("fish-grade-6")] FishGrade6,
		[Name("fish-grade-7")] FishGrade7,
		[Name("fish-grade-8")] FishGrade8,
		[Name("fish-grade-9")] FishGrade9,
		[Name("fish-grade-10")] FishGrade10,
		COUNT
	}
}